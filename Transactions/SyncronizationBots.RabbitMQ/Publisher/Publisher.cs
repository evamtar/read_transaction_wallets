using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher.Interface;
using SyncronizationBots.RabbitMQ.Queue.Interface;
using System.Net.Sockets;
using System.Text;

namespace SyncronizationBots.RabbitMQ.Publisher
{
    public class Publisher : IPublishMessage
    {
        

        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly IQueueConfiguration _queueConfiguration;
        public Publisher(IRabbitMQConnection rabbitMQConnection, IQueueConfiguration queueConfiguration) 
        {
            this._rabbitMQConnection = rabbitMQConnection ?? throw new ArgumentNullException(nameof(this._rabbitMQConnection));
            this._queueConfiguration = queueConfiguration ?? throw new ArgumentNullException(nameof(this._queueConfiguration)); 
        }

        public Task Publish<T>(MessageEvent<T> @event) where T : Entity
        {
            using var channel = this._rabbitMQConnection.CreateModel();
            try
            {
                if (!_rabbitMQConnection.IsConnected)
                {
                    _rabbitMQConnection.TryConnect();
                }

                
                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        throw ex;
                    });
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

                policy.Execute(() =>
                {
                    //PublishMessage
                    channel.BasicPublish(exchange: this._queueConfiguration?.Exchange,
                                         routingKey: this._queueConfiguration?.QueueName,
                                         basicProperties: properties,
                                         mandatory: this._queueConfiguration?.Mandatory ?? false,
                                         body: Encoding.UTF8.GetBytes(@event.JsonSerialize()));
                });
            }
            finally 
            {
                channel.Dispose();
            }
            
            return Task.CompletedTask;
        }
    }
}
