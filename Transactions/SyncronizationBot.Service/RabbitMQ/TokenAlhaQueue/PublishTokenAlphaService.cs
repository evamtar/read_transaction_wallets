using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.TokenAlhaQueue;
using SyncronizationBot.Service.RabbitMQ.TokenAlhaQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.TokenAlhaQueue
{
    public class PublishTokenAlphaService : Publisher, IPublishTokenAlphaService
    {
        public PublishTokenAlphaService(IRabbitMQConnection rabbitMQConnection, IOptions<TokenAlhaQueueConfig> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
