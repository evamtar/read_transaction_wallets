using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers
{
    public class UpdateWalletsBalanceCommandHandler : IRequestHandler<UpdateWalletsBalanceCommand, UpdateWalletsBalanceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletBalanceRepository _walletBalanceRepository;

        public UpdateWalletsBalanceCommandHandler(IMediator mediator, 
                                                  IWalletRepository walletRepository,
                                                  IWalletBalanceRepository walletBalanceRepository)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
            this._walletBalanceRepository = walletBalanceRepository;
        }

        public async Task<UpdateWalletsBalanceCommandResponse> Handle(UpdateWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallets = await this._walletRepository.Get(x => x.IsLoadBalance == true);
            foreach (var wallet in wallets)
            {
                var balances = await this._walletBalanceRepository.Get(x => x.IdWallet == wallet.ID && x.LastUpdate!.Value.AddHours(1) < DateTime.Now);
                if (balances != null && balances.Any()) 
                {
                    var prices = await this._mediator.Send(new RecoveryPriceCommand { Ids = this.GetIdsTokens(balances) });
                    foreach (var balance in balances)
                    {
                        if (prices?.Data?.ContainsKey(balance.TokenHash!) ?? false) 
                        {
                            balance.TotalValueUSD = balance.Quantity * (prices?.Data?[balance.TokenHash!].Price ?? 0);
                            balance.LastUpdate = DateTime.Now;
                            await this._walletBalanceRepository.Edit(balance);
                        }
                    }
                }
            }
            return new UpdateWalletsBalanceCommandResponse { };
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
