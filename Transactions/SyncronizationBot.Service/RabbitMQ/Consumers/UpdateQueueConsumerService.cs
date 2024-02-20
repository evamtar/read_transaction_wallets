using Microsoft.Extensions.Options;
using SyncronizationBot.Service.BatchMessageConsumer.Base;
using SyncronizationBot.Service.RabbitMQ.UpdateQueue.Configs;

namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class UpdateQueueConsumerService : BaseBatchMessageConsumer
    {
        public UpdateQueueConsumerService(IServiceProvider serviceProvider, IOptions<UpdateQueueConfiguration> configuration) : base(serviceProvider, configuration.Value)
        {

        }

        public override Task HandlerAsync(string? message, CancellationToken stoppingToken)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
