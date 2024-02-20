using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.UpdatesQueue;
using SyncronizationBot.Service.RabbitMQ.UpdatesQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;
using SyncronizationBots.RabbitMQ.Queue.Interface;

namespace SyncronizationBot.Service.RabbitMQ.UpdatesQueue
{
    public class PublishUpdateService : Publisher, IPublishUpdateService
    {
        public PublishUpdateService(IRabbitMQConnection rabbitMQConnection, IOptions<UpdateQueueConfiguration> configuration) : base(rabbitMQConnection, (IQueueConfiguration)configuration.Value)
        {
        }
    }
}
