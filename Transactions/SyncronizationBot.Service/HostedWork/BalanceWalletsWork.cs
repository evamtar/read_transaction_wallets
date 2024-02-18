using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Command;
using SyncronizationBot.Application.InsertCommand.Token.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalance.Command;
using SyncronizationBot.Application.InsertCommand.WalletBalanceHistory.Command;
using SyncronizationBot.Application.UpdateCommand.Wallet.Command;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.InternalService.HostedWork;
using SyncronizationBot.Domain.Service.InternalService.HostedWork.Base;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Domain.Service.InternalService.Utils;
using System.Diagnostics;
using System.Xml.Linq;

namespace SyncronizationBot.Service.HostedWork
{
    public class BalanceWalletsWork : IBalanceWalletsWork
    {
        private readonly IMediator _mediator;
        private readonly IPreLoadedEntitiesService _preLoadedEntitiesService;
        private readonly ITokenService _tokenService;
        public IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        public ETypeService? TypeService => ETypeService.Balance;
        
        public BalanceWalletsWork(IMediator mediator,
                                  IPreLoadedEntitiesService preLoadedEntitiesService,
                                  ITokenService tokenService)
        {
            this._mediator = mediator;
            this._preLoadedEntitiesService = preLoadedEntitiesService;
            this._tokenService = tokenService;
        }

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            var wallets = await this._preLoadedEntitiesService.GetFilteredWalletAsync(x => x.IsActive == true && x.IsLoadBalance == false);
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
                                var token = await _tokenService.FindFirstOrDefault(x => x.Hash == balance.Token!.Hash);
                                if (token == null)
                                {
                                    token = new Token
                                    {
                                        Decimals = balance.Token?.Decimals,
                                        Hash = balance.Token?.Hash,
                                        Symbol = balance.Token?.Symbol,
                                        Name = balance.Token?.Name,
                                        IsLazyLoad = true
                                    };
                                    var response = await this._mediator.Send(new TokenInsertCommand { Entity = token });
                                    token = response.Entity;
                                }
                                var walletBalance = new WalletBalance
                                {
                                    WalletId = wallet?.ID,
                                    TokenId = token?.ID,
                                    TokenHash = token?.Hash,
                                    Quantity = balance?.Amount,
                                    Price = null,
                                    TotalValueUSD = null,
                                    IsActive = true,
                                    LastUpdate = DateTime.Now
                                };
                                await this._mediator.Send(new WalletBalanceInsertCommand { Entity = walletBalance });
                                var walletBalanceHistory = new WalletBalanceHistory
                                {
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
                                    FontType = EFontType.SOLANA_RPC,
                                    CreateDate = DateTime.Now,
                                    LastUpdate = walletBalance?.LastUpdate
                                };
                                await this._mediator.Send(new WalletBalanceHistoryInsertCommand { Entity = walletBalanceHistory });
                            }

                        }
                        wallet!.DateLoadBalance = balanceResponse?.DateLoadBalance;
                        wallet!.IsLoadBalance = true;
                        await this._mediator.Send(new WalletUpdateCommand { Entity = wallet });
                    }
                }
            }
        }
    }
}
