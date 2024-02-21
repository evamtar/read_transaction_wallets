using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenAlhaQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenAlhaQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.Queue.TokenAlhaQueue
{
    public class PublishTokenAlphaService : Publisher, IPublishTokenAlphaService
    {
        public PublishTokenAlphaService(IRabbitMQConnection rabbitMQConnection, IOptions<TokenAlhaQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
