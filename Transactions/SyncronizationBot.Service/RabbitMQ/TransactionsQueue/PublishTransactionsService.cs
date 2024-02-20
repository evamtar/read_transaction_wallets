using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.TransactionsQueue;
using SyncronizationBot.Service.RabbitMQ.TransactionsQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.TransactionsQueue
{
    public class PublishTransactionsService : Publisher, IPublishTransactionsService
    {
        public PublishTransactionsService(IRabbitMQConnection rabbitMQConnection, IOptions<TransactionsQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
