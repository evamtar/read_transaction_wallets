using MediatR;
using SyncronizationBot.Application.Base;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using System.Diagnostics;


namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsBalanceCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>
    {
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        private readonly IWalletPortifolioService _walletPortifolioService;
        private readonly IAccountInfoService _accountInfoService;


        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IWalletRepository walletRepository,
                                                IWalletBalanceRepository walletBalanceRepository,
                                                IWalletPortifolioService walletPortifolioService,
                                                IAccountInfoService accountInfoService) : base(mediator, walletRepository)
        {
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletPortifolioService = walletPortifolioService;
            this._accountInfoService = accountInfoService;
        }
        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallets = await base.GetWallets(x => x.IsLoadBalance == false && x.IsActive == true);
            foreach (var wallet in wallets)
            {
                var accountInfo = await this._accountInfoService.ExecuteRecoveryAccountInfoAsync(new AccountInfoRequest { WalletHash = wallet.Hash });
                if (accountInfo != null && accountInfo.Result?.Value?.Lamports > 0) 
                {
                    var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                    await this._walletBalanceRepository.Add(new WalletBalance
                    {
                        IdWallet = wallet.ID,
                        IdToken = token?.TokenId,
                        TokenHash = "So11111111111111111111111111111111111111112",
                        Quantity = accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals),
                        Price = token?.MarketCap / token?.Supply,
                        TotalValueUSD = (accountInfo.Result?.Value?.Lamports / this.GetDivisor(token?.Decimals)) * (token?.MarketCap / token?.Supply),
                        IsActive = accountInfo.Result?.Value?.Lamports > 0,
                        LastUpdate = DateTime.Now
                    });
                }
                var finalTicks = base.GetFinalTicks();
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
                await base.UpdateUnixTimeSeconds(finalTicks, wallet);
            }
            return new ReadWalletsBalanceCommandResponse { };
        }

        private decimal? GetDivisor(int? decimals) 
        {
            string number = "1";
            for (int i = 0; i < decimals; i++)
                number += "0";
            return decimal.Parse(number);
        }
    }
}
