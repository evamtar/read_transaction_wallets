using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.UpdateQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.UpdateQueue
{
    public class PublishUpdateService : Publisher, IPublishUpdateService
    {
        public PublishUpdateService(IRabbitMQConnection rabbitMQConnection, IOptions<UpdateQueueConfiguration> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
