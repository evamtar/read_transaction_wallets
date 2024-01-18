using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using System.Diagnostics;


namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsBalanceCommandHandler : IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        private readonly IWalletPortifolioService _walletPortifolioService;
        
        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IWalletRepository walletRepository,
                                                IWalletBalanceRepository walletBalanceRepository,
                                                IWalletPortifolioService walletPortifolioService) 
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletPortifolioService = walletPortifolioService;
        }
        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallets = await this._walletRepository.Get(x => x.IsLoadBalance == false && x.IsActive == true);
            foreach (var wallet in wallets)
            {
                var walletPortifolio = await this._walletPortifolioService.ExecuteRecoveryWalletPortifolioAsync(new WalletPortifolioRequest { WalletHash = wallet.Hash });
                if (walletPortifolio?.Data?.Items != null) 
                {
                    foreach (var item in walletPortifolio!.Data!.Items)
                    {
                        var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = item.Address });
                        await this._walletBalanceRepository.Add(new WalletBalance
                        {
                            IdWallet = wallet.ID,
                            IdToken = token?.TokenId,
                            TokenHash = item.Address,
                            Quantity = item.UiAmount,
                            Price = item.PriceUsd,
                            TotalValueUSD = item.ValueUsd,
                            IsActive = item.UiAmount > 0,
                            LastUpdate = DateTime.Now
                        });
                    }
                }
                wallet.IsLoadBalance = true;
                await this._walletRepository.Edit(wallet);
                try { await this._walletRepository.DetachedItem(wallet); } catch { }
            }
            return new ReadWalletsBalanceCommandResponse { };
        }
    }
}
