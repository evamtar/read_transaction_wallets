using Microsoft.Extensions.DependencyInjection;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Consumer;
using SyncronizationBots.RabbitMQ.Queue.Interface;

namespace SyncronizationBot.Service.BatchMessageConsumer.Base
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
            this._scope = serviceProvider.CreateScope();
            this.RabbitMQConnection = this._scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
            this.QueueConfiguration = queueConfiguration;
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
