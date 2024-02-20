using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Queue.Interface;
using System.Text;
using System.Threading;

namespace SyncronizationBots.RabbitMQ.Consumer
{
    public abstract class ConsumerBackgroundService : BackgroundService
    {
        public abstract IRabbitMQConnection RabbitMQConnection { get; set; }
        public abstract IQueueConfiguration QueueConfiguration { get; set; }

        private IModel _consumerChannel;

        public ConsumerBackgroundService()
        {
            this._consumerChannel = null!;
        }

        public abstract Task HandlerAsync(string? message, CancellationToken stoppingToken);
        public abstract Task LogInfo(string? info);

        public virtual void StartBasicConsumer()
        {
            if (this._consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(this._consumerChannel);
                consumer.Received += ConsumerReceived;
                this._consumerChannel.BasicConsume(this.QueueConfiguration.QueueName, false, consumer);
            }
            else 
                throw new ArgumentNullException(nameof(this._consumerChannel));
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._consumerChannel = CreateConsumerChannel();
            this.StartBasicConsumer();
            while (!stoppingToken.IsCancellationRequested) 
            {
                await LogInfo($" Active --> Exchange: {this.QueueConfiguration.Exchange} | QueueName: {this.QueueConfiguration.QueueName}");
                await Task.Delay(30000);
            }
        }

        private async void ConsumerReceived(object? sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var message = Encoding.UTF8.GetString(@event.Body.ToArray());
                await HandlerAsync(message, CancellationToken.None);
                this._consumerChannel.BasicAck(@event.DeliveryTag, false);
            }
            catch(Exception ex)
            {
                await LogInfo(@$" Exceção --> {ex.Message}
                                  StackTrace: {ex.StackTrace}
                                  InnerException: {ex.InnerException}
                                  InnerException---> Message: {ex.InnerException?.Message}
                                  InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                this._consumerChannel.BasicNack(@event.DeliveryTag, false, false);
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!this.RabbitMQConnection.IsConnected)
            {
                this.RabbitMQConnection.TryConnect();
            }

            var channel = this.RabbitMQConnection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            channel.ExchangeDeclare(exchange: this.QueueConfiguration?.Exchange,
                                    type: "direct",
                                    durable: this.QueueConfiguration?.Durable ?? false,
                                    autoDelete: this.QueueConfiguration?.AutoDelete ?? false);

            //Declare Queue
            channel.QueueDeclare(queue: this.QueueConfiguration?.QueueName,
                                 durable: this.QueueConfiguration?.Durable ?? false,
                                 exclusive: this.QueueConfiguration?.Exclusive ?? false,
                                 autoDelete: this.QueueConfiguration?.AutoDelete ?? false,
                                 arguments: this.QueueConfiguration?.Arguments);

            channel.CallbackException += (sender, ea) =>
            {
                LogInfo(ea.Exception.Message);
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsumer();
            };

            return channel;
        }

        public override void Dispose()
        {
            try
            {
                if (_consumerChannel != null)
                {
                    _consumerChannel.Dispose();
                }
            }
            finally 
            { 
                base.Dispose();
            }
        }
    }
}
