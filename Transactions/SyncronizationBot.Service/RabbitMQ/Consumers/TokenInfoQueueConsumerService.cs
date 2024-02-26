using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue.Configs;
using SyncronizationBots.RabbitMQ.Connection.Interface;

namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class TokenInfoQueueConsumerService : BaseBatchMessageConsumer
    {
        private IUnitOfWorkSqlServer UnitOfWorkSqlServer { get; set; }
        private IMediator Mediator { get; set; }
        public TokenInfoQueueConsumerService(IServiceProvider serviceProvider,
                                             IRabbitMQConnection rabbitMQConnection,
                                             IOptions<TokenInfoQueueConfiguration> configuration) : base(serviceProvider, rabbitMQConnection, configuration.Value)
        {
            this.UnitOfWorkSqlServer = null!;
            this.Mediator = null!;
        }
        public override async Task HandlerAsync(IServiceScope _scope, string? message, CancellationToken stoppingToken) 
        {
            this.InitServices(_scope);
            var @event = JsonConvert.DeserializeObject<MessageEvent<Token>>(message ?? string.Empty);
            if (@event?.Entity?.IsLazyLoad ?? false) 
            { 

            }
            
        }

        private void InitServices(IServiceScope _scope)
        {
            this.Mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
            this.UnitOfWorkSqlServer = _scope.ServiceProvider.GetRequiredService<IUnitOfWorkSqlServer>();
        }
    }
}
