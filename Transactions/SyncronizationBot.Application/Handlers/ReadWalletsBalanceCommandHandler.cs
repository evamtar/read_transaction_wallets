using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Configs;
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
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IWalletPortifolioService _walletPortifolioService;
        private readonly IAccountInfoService _accountInfoService;


        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IWalletRepository walletRepository,
                                                IOptions<SyncronizationBotConfig> config,
                                                IWalletBalanceRepository walletBalanceRepository,
                                                IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                IWalletPortifolioService walletPortifolioService,
                                                IAccountInfoService accountInfoService) : base(mediator, walletRepository, config)
        {
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._walletPortifolioService = walletPortifolioService;
            this._accountInfoService = accountInfoService;
        }
        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallet = await base.GetWallet(x => x.IsLoadBalance == false && x.IsActive == true);
            var hasNext = wallet != null;
            while (hasNext) 
            {
                var token = (RecoverySaveTokenCommandResponse)null!;
                var finalTicks = base.GetInitialTicks(base.GetFinalTicks());
                var walletPortifolio = await this._walletPortifolioService.ExecuteRecoveryWalletPortifolioAsync(new WalletPortifolioRequest { WalletHash = wallet!.Hash });
                wallet!.DateLoadBalance = DateTime.Now;
                if (walletPortifolio?.Data?.Items != null)
                {
                    token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                    var checkedExists = await this._walletBalanceRepository.FindFirstOrDefault(x => x.IdWallet == wallet!.ID && x.IdToken == token.TokenId);
                    if (checkedExists == null)
                    {
                        var accountInfo = await this._accountInfoService.ExecuteRecoveryAccountInfoAsync(new AccountInfoRequest { WalletHash = wallet!.Hash });
                        if (accountInfo != null && accountInfo.Result?.Value?.Lamports > 0)
                        {
                            var balance = await this._walletBalanceRepository.Add(new WalletBalance
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
                            await this._walletBalanceHistoryRepository.Add(new WalletBalanceHistory 
                            {
                                IdWalletBalance = balance.ID,
                                IdWallet = balance.IdWallet,
                                IdToken = balance.IdToken,
                                TokenHash = balance.TokenHash,
                                OldQuantity = (decimal?)0,
                                NewQuantity = balance.Quantity,
                                RequestQuantity = balance.Quantity,
                                PercentageCalculated = 100,
                                Price = balance.Price,
                                TotalValueUSD = balance.TotalValueUSD,
                                Signature = "CREATE BALANCE",
                                CreateDate = DateTime.Now,
                                LastUpdate = balance.LastUpdate
                            });
                        }
                    }
                    foreach (var item in walletPortifolio!.Data!.Items)
                    {
                        if (item.Address == "So11111111111111111111111111111111111111111")
                            continue;
                        else
                            token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = item.Address });
                        var balance = await this._walletBalanceRepository.Add(new WalletBalance
                        {
                            IdWallet = wallet?.ID,
                            IdToken = token?.TokenId,
                            TokenHash = item.Address == "So11111111111111111111111111111111111111111" ? "So11111111111111111111111111111111111111112" : item.Address,
                            Quantity = item.UiAmount,
                            Price = item.PriceUsd,
                            TotalValueUSD = item.ValueUsd,
                            IsActive = item.UiAmount > 0,
                            LastUpdate = wallet?.DateLoadBalance
                        });
                        await this._walletBalanceHistoryRepository.Add(new WalletBalanceHistory
                        {
                            IdWalletBalance = balance.ID,
                            IdWallet = balance.IdWallet,
                            IdToken = balance.IdToken,
                            TokenHash = balance.TokenHash,
                            OldQuantity = (decimal?)0,
                            NewQuantity = balance.Quantity,
                            RequestQuantity = balance.Quantity,
                            PercentageCalculated = 100,
                            Price = balance.Price,
                            TotalValueUSD = balance.TotalValueUSD,
                            Signature = "CREATE BALANCE",
                            CreateDate = DateTime.Now,
                            LastUpdate = balance.LastUpdate
                        });
                    }
                }
                wallet!.IsLoadBalance = true;
                await base.UpdateUnixTimeSeconds(finalTicks, wallet);
                wallet = await base.GetWallet(x => x.IsLoadBalance == false && x.IsActive == true);
                hasNext = wallet != null;
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
