﻿using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.HostedWork;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.TokenInfoQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Domain.Service.RecoveryService.Wallet;
using SyncronizationBot.Service.HostedWork.Base;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Service.HostedWork
{
    public class BalanceWalletsUpdateWork : BaseWorkForUpdate, IBalanceWalletsUpdateWork
    {
        private readonly IMediator _mediator;
        private readonly IWalletService _walletService;
        private readonly ITokenService _tokenService;
        private readonly IWalletBalanceService _walletBalanceService;
        private readonly IWalletBalanceHistoryService _walletBalanceHistoryService;

        public BalanceWalletsUpdateWork(IMediator mediator,
                                        IWalletService walletService,
                                        ITokenService tokenService,
                                        IWalletBalanceService walletBalanceService,
                                        IWalletBalanceHistoryService walletBalanceHistoryService,
                                        IPublishUpdateService publishUpdateService) : base(publishUpdateService)
        {
            this._mediator = mediator;
            this._walletService = walletService;
            this._tokenService = tokenService;
            this._walletBalanceService = walletBalanceService;
            this._walletBalanceHistoryService = walletBalanceHistoryService;
        }
        public IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        public ETypeService? TypeService => ETypeService.BalanceUpdate;

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            var tokensForUpdate = await this._walletBalanceService.GetAllTokensForUpdateAsync(x => x.IsActive == true);
            var maxCount = 5;
            var count = 0;
            //For services limitation
            foreach (var token in tokensForUpdate)
            {
                if (count == maxCount)
                {
                    count = 0;
                    await Task.Delay(40000);
                }
                var tokenPrice = await this._mediator.Send(new ReadTokenPriceCommand { TokenHash = token.Hash! });
                if (tokenPrice.PriceUsd > 0) 
                {
                    var tokenPriceHistory = new TokenPriceHistory 
                    { 
                        TokenId = token.ID,
                        MarketCap = tokenPrice.Marketcap,
                        Liquidity = tokenPrice.Liquidity,
                        UniqueWallet24h = (int?)tokenPrice.UniqueWallet24H,
                        UniqueWalletHistory24h = (int?)tokenPrice.UniqueWalletHistory24H,
                        NumberMarkets = (int?)tokenPrice.NumberOfMarkets,
                        PriceUsd = tokenPrice.PriceUsd,
                        PriceChangePercent5m = tokenPrice.PriceChange5m,
                        PriceChangePercent30m = tokenPrice.PriceChange30m,
                        PriceChangePercent1h = tokenPrice.PriceChange1h,
                        PriceChangePercent4h = tokenPrice.PriceChange4h,
                        PriceChangePercent6h = tokenPrice.PriceChange6h,
                        PriceChangePercent24h = tokenPrice.PriceChange24,
                        FontPrice = tokenPrice.FontPrice,
                        CreateDate = DateTime.Now,
                    };
                    await base.PublishMessage(tokenPriceHistory, Constants.INSTRUCTION_INSERT); //Validar se a instrução existe
                    await base.PublishMessage(tokenPriceHistory, Constants.INSTRUCTION_UPDATE_RANGE_WB); // Cria a instrução
                    this._walletBalanceService.UpdateAllBalancesWithToken(tokenPriceHistory);
                }
                count++;
            }
        }

        public void Dispose()
        {
            try
            {
                this._walletService.Dispose();
                this._tokenService.Dispose();
                this._walletBalanceService.Dispose();
                this._walletBalanceHistoryService.Dispose();
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
