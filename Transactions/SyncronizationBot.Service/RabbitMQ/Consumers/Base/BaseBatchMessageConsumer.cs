using Microsoft.Extensions.DependencyInjection;
using SyncronizationBots.RabbitMQ.Connection.Interface;
using SyncronizationBots.RabbitMQ.Consumer;
using SyncronizationBots.RabbitMQ.Queue.Interface;
using System;

namespace SyncronizationBot.Service.RabbitMQ.Consumers.Base
{
    public abstract class BaseBatchMessageConsumer : ConsumerBackgroundService
    {
        #region Readonly Variables

        private readonly IServiceProvider _serviceProvider;
        public override IRabbitMQConnection RabbitMQConnection { get; set; }
        public override IQueueConfiguration QueueConfiguration { get; set; }

        #endregion

        public BaseBatchMessageConsumer(IServiceProvider serviceProvider,
                                        IRabbitMQConnection connection,
                                        IQueueConfiguration queueConfiguration)
        {
            _serviceProvider = serviceProvider;
            RabbitMQConnection = connection;
            QueueConfiguration = queueConfiguration;
        }
        public abstract Task HandlerAsync(IServiceScope _scope, string? message, CancellationToken stoppingToken);

        public override async Task HandlerAsync(string? message, CancellationToken stoppingToken)
        {
            using (var _scope = _serviceProvider.CreateScope()) 
                await HandlerAsync(_scope, message, stoppingToken);
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
                RabbitMQConnection.Dispose();
            }
            finally
            {
            }
            return base.StopAsync(cancellationToken);
        }
    }
}
