using Microsoft.Extensions.Options;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.UpdateQueue.Configs;

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
