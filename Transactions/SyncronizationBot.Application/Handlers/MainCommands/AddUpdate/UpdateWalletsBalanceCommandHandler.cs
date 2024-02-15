using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.AddUpdate
{
    public class UpdateWalletsBalanceCommandHandler : BaseWalletHandler, IRequestHandler<UpdateWalletsBalanceCommand, UpdateWalletsBalanceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceRepository _walletBalanceRepository;

        public UpdateWalletsBalanceCommandHandler(IMediator mediator,
                                                  IWalletRepository walletRepository,
                                                  IOptions<SyncronizationBotConfig> config,
                                                  IWalletBalanceRepository walletBalanceRepository) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._mediator = mediator;
            this._walletBalanceRepository = walletBalanceRepository;
        }

        public async Task<UpdateWalletsBalanceCommandResponse> Handle(UpdateWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var dateTimeLimit = DateTimeOffset.Now;
            var walletsTracked = await GetWallets(x => x.IsActive == true && x.IsLoadBalance == true, x => x.DateLoadBalance!);
            if(walletsTracked?.Count() > 0) 
            {
                foreach (var walletTracked in walletsTracked)
                {
                    var balances = await this._walletBalanceRepository.Get(x => x.WalletId == walletTracked!.ID && x.TokenId != null && x.IsActive == true && x.LastUpdate!.Value.AddHours(1) < dateTimeLimit) ;
                    if (balances?.Count() > 0) 
                    {
                        var prices = await this._mediator.Send(new RecoveryPriceCommand { Ids = this.GetIdsTokens(balances) });
                        foreach (var balance in balances)
                        {
                            if (prices?.Data?.ContainsKey(balance.TokenHash!) ?? false) { 
                                balance.TotalValueUSD = balance.Quantity * (prices?.Data?[balance.TokenHash!].Price ?? 0);
                                balance.Price = prices?.Data?[balance.TokenHash!].Price ?? 0;
                            }
                            else 
                            {
                                var token = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = balance!.TokenHash! });
                                balance.TotalValueUSD = balance.Quantity * (token.Price ?? 0);
                                balance.Price = token?.Price ?? 0;
                            }
                            balance.LastUpdate = DateTime.Now;
                            await this._walletBalanceRepository.Edit(balance);
                            await this._walletBalanceRepository.DetachedItem(balance);
                            await UpdateBalancesWithSameToken(balance.ID, balance.TokenId, balance.Price);
                        }
                    }
                }
            }
            return new UpdateWalletsBalanceCommandResponse { };
        }

        private async Task UpdateBalancesWithSameToken(Guid? balanceId, Guid? tokenId, decimal? price)
        {
            var balancesWithSameToken = await this._walletBalanceRepository.Get(x => x.ID != balanceId && x.TokenId == tokenId && x.IsActive == true);
            if(balancesWithSameToken?.Count > 0) 
            { 
                foreach (var balance in balancesWithSameToken) 
                {
                    balance.TotalValueUSD = balance.Quantity * (price ?? 0);
                    balance.Price = (price ?? 0);
                    balance.LastUpdate = DateTime.Now;
                    await this._walletBalanceRepository.Edit(balance);
                    await this._walletBalanceRepository.DetachedItem(balance);
                }
            }
        }

        private List<string> GetIdsTokens(IEnumerable<WalletBalance> walletBalances)
        {
            var listIdsTokens = new List<string> { };
            foreach (var walletBalance in walletBalances)
                listIdsTokens.Add(walletBalance.TokenHash!);
            return listIdsTokens;
        }
    }
}
