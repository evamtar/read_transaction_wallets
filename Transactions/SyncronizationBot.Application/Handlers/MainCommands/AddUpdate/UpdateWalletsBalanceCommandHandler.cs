using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.AddUpdate
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
            _mediator = mediator;
            _walletRepository = walletRepository;
            _walletBalanceRepository = walletBalanceRepository;
        }

        public async Task<UpdateWalletsBalanceCommandResponse> Handle(UpdateWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.Get(x => x.IsLoadBalance == true);
            foreach (var wallet in wallets)
            {
                var balances = await _walletBalanceRepository.Get(x => x.IdWallet == wallet.ID && x.IdToken != null && x.IsActive == true && x.LastUpdate!.Value.AddHours(1) < DateTime.Now);
                if (balances != null && balances.Any())
                {
                    var prices = await _mediator.Send(new RecoveryPriceCommand { Ids = GetIdsTokens(balances) });
                    foreach (var balance in balances)
                    {
                        if (prices?.Data?.ContainsKey(balance.TokenHash!) ?? false)
                            balance.TotalValueUSD = balance.Quantity * (prices?.Data?[balance.TokenHash!].Price ?? 0);
                        balance.LastUpdate = DateTime.Now;
                        await _walletBalanceRepository.Edit(balance);
                        try { await _walletBalanceRepository.DetachedItem(balance); } catch { }
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
