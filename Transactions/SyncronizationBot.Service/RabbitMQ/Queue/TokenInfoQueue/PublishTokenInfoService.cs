using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenInfoQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;

namespace SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue
{
    public class PublishTokenInfoService : Publisher, IPublishTokenInfoService
    {
        public PublishTokenInfoService(IRabbitMQConnection rabbitMQConnection, IOptions<TokenInfoQueueConfiguration> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
