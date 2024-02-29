using Microsoft.Extensions.DependencyInjection;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.Base;
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
        private IServiceScope Scope { get; set; }
        public override IRabbitMQConnection RabbitMQConnection { get; set; }
        public override IQueueConfiguration QueueConfiguration { get; set; }
        
        #endregion

        public BaseBatchMessageConsumer(IServiceProvider serviceProvider,
                                        IQueueConfiguration queueConfiguration)
        {
            _serviceProvider = serviceProvider;
            QueueConfiguration = queueConfiguration;
            this.RabbitMQConnection = null!;
            this.Scope = null!;
            InitServices();
        }

        public abstract Task HandlerAsync(IServiceScope scope, string? message, CancellationToken stoppingToken);

        public override async Task HandlerAsync(string? message, CancellationToken stoppingToken)
        {
            await HandlerAsync(this.Scope, message, stoppingToken);
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

        protected virtual async Task TransferQueue<T, W>(T entity, string? instruction, W transferQueueService) where T : Entity
                                                                                        where W: IPublishBaseService
        {
            await transferQueueService.Publish(new MessageEvent<T>
            {
                CreateDate = DateTime.Now,
                Entity = entity,
                EventName = typeof(T).Name + "_" + instruction,
                Parameters = null
            });
        }

        public override void Dispose()
        {
            try
            {
                this.Scope.Dispose();
            }
            finally 
            {
                base.Dispose();
            }
        }

        private void InitServices()
        {
            this.Scope = this._serviceProvider.CreateScope();
            this.RabbitMQConnection = this.Scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
        }
    }
}
