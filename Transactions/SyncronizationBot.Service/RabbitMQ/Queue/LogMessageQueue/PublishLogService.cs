using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.LogMessageQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.LogMessageQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;


namespace SyncronizationBot.Service.RabbitMQ.Queue.LogMessageQueue
{
    public class PublishLogService : Publisher, IPublishLogService
    {
        public PublishLogService(IRabbitMQConnection rabbitMQConnection, IOptions<LogMessageQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
