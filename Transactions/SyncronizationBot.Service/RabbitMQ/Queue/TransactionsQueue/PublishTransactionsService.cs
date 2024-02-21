using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TransactionsQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TransactionsQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.Queue.TransactionsQueue
{
    public class PublishTransactionsService : Publisher, IPublishTransactionsService
    {
        public PublishTransactionsService(IRabbitMQConnection rabbitMQConnection, IOptions<TransactionsQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
