using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Queue.Interface;
using System.Text;

namespace SyncronizationBots.RabbitMQ.Consumer
{
    public abstract class Consumer<T> : IDisposable where T : Entity
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly IQueueConfiguration _queueConfiguration;
        private IModel _consumerChannel;

        public Consumer(IRabbitMQConnection rabbitMQConnection, 
                        IQueueConfiguration queueConfiguration)
        {
            this._rabbitMQConnection = rabbitMQConnection ?? throw new ArgumentNullException(nameof(this._rabbitMQConnection));
            this._queueConfiguration = queueConfiguration ?? throw new ArgumentNullException(nameof(this._queueConfiguration));
            this._consumerChannel = CreateConsumerChannel();
        }

        public virtual void StartBasicConsumer()
        {
            if (this._consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(this._consumerChannel);
                consumer.Received += ConsumerReceived<T>;
                this._consumerChannel.BasicConsume(this._queueConfiguration.QueueName, false, consumer);
            }
            else 
                throw new ArgumentNullException(nameof(this._consumerChannel));
            
        }

        public abstract Task ExecuteAsync<W>(MessageEvent<W>? message) where W : Entity;
        private async Task ConsumerReceived<W>(object sender, BasicDeliverEventArgs eventArgs) where W : Entity
        {
            var message = JsonConvert.DeserializeObject<MessageEvent<W>>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
            await this.Process(message, eventArgs.DeliveryTag, false, false);
        }

        private async Task Process<W>(MessageEvent<W>? message, ulong deliveryTag, bool multiple, bool requeue) where W : Entity
        {
            try
            {
                await ExecuteAsync(message);
                this._consumerChannel.BasicAck(deliveryTag, multiple);
            }
            catch 
            {
                this._consumerChannel.BasicNack(deliveryTag, multiple, requeue);
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!this._rabbitMQConnection.IsConnected)
            {
                this._rabbitMQConnection.TryConnect();
            }

            var channel = this._rabbitMQConnection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent

            channel.ExchangeDeclare(exchange: this._queueConfiguration?.Exchange,
                                    type: "direct",
                                    durable: this._queueConfiguration?.Durable ?? false,
                                    autoDelete: this._queueConfiguration?.AutoDelete ?? false);

            //Declare Queue
            channel.QueueDeclare(queue: this._queueConfiguration?.QueueName,
                                 durable: this._queueConfiguration?.Durable ?? false,
                                 exclusive: this._queueConfiguration?.Exclusive ?? false,
                                 autoDelete: this._queueConfiguration?.AutoDelete ?? false,
                                 arguments: this._queueConfiguration?.Arguments);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsumer();
            };

            return channel;
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }
}
