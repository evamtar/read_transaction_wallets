using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue
{
    public class PublishUpdateService : Publisher, IPublishUpdateService
    {
        public PublishUpdateService(IRabbitMQConnection rabbitMQConnection, IOptions<UpdateQueueConfiguration> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
