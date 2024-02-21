using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.AlertPriceQueue;
using SyncronizationBot.Service.RabbitMQ.Queue.AlertPriceQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Publisher;


namespace SyncronizationBot.Service.RabbitMQ.Queue.AlertPriceQueue
{
    public class PublishAlertPriceService : Publisher, IPublishAlertPriceService
    {
        public PublishAlertPriceService(IRabbitMQConnection rabbitMQConnection, IOptions<AlertPriceQueueConfiguration> configuration) : base(rabbitMQConnection, configuration.Value)
        {
        }
    }
}
