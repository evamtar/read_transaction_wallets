using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Command;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Service.RabbitMQ.Consumers.Base;
using SyncronizationBot.Service.RabbitMQ.Queue.TokenInfoQueue.Configs;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Service.RabbitMQ.Consumers
{
    public class TokenInfoQueueConsumerService : BaseBatchMessageConsumer
    {
        public IPublishUpdateService PublishUpdateService { get; set; }
        private IMediator Mediator { get; set; }
        public TokenInfoQueueConsumerService(IServiceProvider serviceProvider,
                                             IOptions<TokenInfoQueueConfiguration> configuration) : base(serviceProvider, configuration.Value)
        {
            this.Mediator = null!;
            this.PublishUpdateService = null!;
        }
        public override async Task HandlerAsync(IServiceScope _scope, string? message, CancellationToken stoppingToken) 
        {
            this.InitServices(_scope);
            var @event = JsonConvert.DeserializeObject<MessageEvent<Token>>(message ?? string.Empty);
            if (@event?.Entity?.IsLazyLoad ?? false) 
            {
                var tokenForChange = @event.Entity;
                var tokenFullInfo = await this.Mediator.Send(new ReadTokenFullInfoCommand { TokenHash = tokenForChange.Hash! });
                tokenForChange.Name = tokenFullInfo.Name;
                tokenForChange.Symbol  = tokenFullInfo.Symbol;
                tokenForChange.Supply = tokenFullInfo.Supply;
                tokenForChange.Decimals = (int?)tokenFullInfo.Decimals;
                tokenForChange.CreateDate = tokenFullInfo.CreateDate;
                tokenForChange.LastUpdate = DateTime.Now;
                tokenForChange.IsLazyLoad = false;
                await base.TransferQueue(tokenForChange, Constants.INSTRUCTION_UPDATE, this.PublishUpdateService);
                var tokenSecurity = new TokenSecurity
                {
                    TokenId = tokenForChange.ID,
                    CreationTime = DateTimeTicks.Instance.ConvertDateTimeToTicks(tokenForChange.CreateDate ?? DateTime.UtcNow.AddHours(-5)),
                    FreezeAuthority = tokenFullInfo.FreezeAuthority,
                    MintAuthority = tokenFullInfo.MintAuthority
                };
                await base.TransferQueue(tokenSecurity, Constants.INSTRUCTION_INSERT, this.PublishUpdateService);
                var tokenPriceHistory = new TokenPriceHistory
                {
                    TokenId = tokenForChange.ID,
                    MarketCap = tokenFullInfo.Marketcap,
                    Liquidity = tokenFullInfo.Liquidity,
                    UniqueWallet24h = (int?)tokenFullInfo.UniqueWallet24H,
                    UniqueWalletHistory24h = (int?)tokenFullInfo.UniqueWalletHistory24H,
                    NumberMarkets = (int?)tokenFullInfo.NumberOfMarkets,
                    CreateDate = DateTime.Now,
                };
                await base.TransferQueue(tokenPriceHistory, Constants.INSTRUCTION_INSERT, this.PublishUpdateService);
            }
        }

        

        private void InitServices(IServiceScope _scope)
        {
            this.Mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
            this.PublishUpdateService = _scope.ServiceProvider.GetRequiredService<IPublishUpdateService>();
        }
    }
}
