using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Command;
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
    public class BalanceWalletsWork : BaseWorkForUpdate, IBalanceWalletsWork
    {
        private readonly IMediator _mediator;
        private readonly IWalletService _walletService;
        private readonly ITokenService _tokenService;
        private readonly IWalletBalanceService _walletBalanceService;
        private readonly IWalletBalanceHistoryService _walletBalanceHistoryService;
        private readonly IPublishTokenInfoService _publishTokenInfoService;
        public IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        public ETypeService? TypeService => ETypeService.Balance;
        
        public BalanceWalletsWork(IMediator mediator,
                                  IWalletService walletService,
                                  ITokenService tokenService,
                                  IWalletBalanceService walletBalanceService,
                                  IWalletBalanceHistoryService walletBalanceHistoryService,
                                  IPublishUpdateService publishUpdateService,
                                  IPublishTokenInfoService publishTokenInfoService) : base(publishUpdateService) 
        {
            this._mediator = mediator;
            this._walletService = walletService;
            this._tokenService = tokenService;
            this._walletBalanceService = walletBalanceService;
            this._walletBalanceHistoryService = walletBalanceHistoryService;
            this._publishTokenInfoService = publishTokenInfoService;
        }

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            var wallets = await this._walletService.GetAsync(x => x.IsActive == true && x.IsLoadBalance == false);
            if (wallets?.Count() > 0)
            {
                foreach (var wallet in wallets)
                {
                    var balanceResponse = await this._mediator.Send(new SolnetBalanceReadCommand { WalletHash = wallet.Hash, IgnoreAmountValueZero = true });
                    if (balanceResponse.IsSuccess ?? false) 
                    {
                        if (balanceResponse?.Result?.Count > 0) 
                        {
                            foreach (var balance in balanceResponse?.Result!)
                            {
                                var token = await this._tokenService.FindFirstOrDefaultAsync(x => x.Hash == balance.Token!.Hash);
                                if (token == null)
                                {
                                    token = new Token
                                    {
                                        ID = Guid.NewGuid(),
                                        Decimals = balance.Token?.Decimals,
                                        Hash = balance.Token?.Hash,
                                        Symbol = balance.Token?.Symbol,
                                        Name = balance.Token?.Name,
                                        IsLazyLoad = true
                                    };
                                    this._tokenService.Add(token);
                                    await base.PublishMessage(token, Constants.INSTRUCTION_INSERT);
                                }
                                var walletBalance = new WalletBalance
                                {
                                    ID = Guid.NewGuid(),
                                    WalletId = wallet?.ID,
                                    TokenId = token?.ID,
                                    TokenHash = token?.Hash,
                                    Quantity = balance?.Amount,
                                    Price = null,
                                    TotalValueUSD = null,
                                    IsActive = true,
                                    LastUpdate = DateTime.Now
                                };
                                this._walletBalanceService.Add(walletBalance);
                                await base.PublishMessage(walletBalance, Constants.INSTRUCTION_INSERT);
                                var walletBalanceHistory = new WalletBalanceHistory
                                {
                                    ID = Guid.NewGuid(),
                                    WalletBalanceId = walletBalance?.ID,
                                    WalletId = walletBalance?.WalletId,
                                    TokenId = walletBalance?.TokenId,
                                    TokenHash = walletBalance?.TokenHash,
                                    OldQuantity = (decimal?)0,
                                    NewQuantity = walletBalance?.Quantity,
                                    RequestQuantity = walletBalance?.Quantity,
                                    PercentageCalculated = 100,
                                    Price = walletBalance?.Price,
                                    TotalValueUSD = walletBalance?.TotalValueUSD,
                                    Signature = "CREATE BALANCE",
                                    CreateDate = DateTime.Now,
                                    LastUpdate = walletBalance?.LastUpdate
                                };
                                this._walletBalanceHistoryService.Add(walletBalanceHistory);
                                await base.PublishMessage(walletBalanceHistory, Constants.INSTRUCTION_INSERT);
                            }
                        }
                        wallet!.DateLoadBalance = balanceResponse?.DateLoadBalance;
                        wallet!.IsLoadBalance = true;
                        this._walletService.Update(wallet);
                        this._walletService.SaveChanges();
                        await base.PublishMessage(wallet, Constants.INSTRUCTION_UPDATE);
                    }
                }
            }
        }
    }
}
