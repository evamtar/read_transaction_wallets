using Microsoft.Extensions.DependencyInjection;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Consumer;
using SyncronizationBots.RabbitMQ.Queue.Interface;

namespace SyncronizationBot.Service.RabbitMQ.Consumers.Base
{
    public abstract class BaseBatchMessageConsumer : ConsumerBackgroundService
    {
        #region Readonly Variables

        private readonly IServiceScope _scope;
        public override IRabbitMQConnection RabbitMQConnection { get; set; }
        public override IQueueConfiguration QueueConfiguration { get; set; }

        #endregion

        public BaseBatchMessageConsumer(IServiceProvider serviceProvider,
                                        IQueueConfiguration queueConfiguration)
        {
            _scope = serviceProvider.CreateScope();
            RabbitMQConnection = _scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
            QueueConfiguration = queueConfiguration;
        }

        public override Task LogInfo(string? info)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(info);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _scope.Dispose();
            }
            finally
            {
            }
            return base.StopAsync(cancellationToken);
        }
    }
}
