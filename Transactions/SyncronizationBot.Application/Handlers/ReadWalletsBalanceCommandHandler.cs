using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Base;
using SyncronizationBot.Application.Commands;
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
        private readonly IWalletPortifolioService _walletPortifolioService;
        

        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IWalletRepository walletRepository,
                                                IOptions<SyncronizationBotConfig> config,
                                                IWalletBalanceRepository walletBalanceRepository,
                                                IWalletPortifolioService walletPortifolioService) : base(mediator, walletRepository, config)
        {
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletPortifolioService = walletPortifolioService;
        }
        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallet = await base.GetWallet(x => x.IsLoadBalance == false && x.IsActive == true);
            var hasNext = wallet != null;
            while (hasNext) 
            {
                var finalTicks = base.GetFinalTicks();
                wallet!.DateLoadBalance = DateTime.Now;
                var walletPortifolio = await this._walletPortifolioService.ExecuteRecoveryWalletPortifolioAsync(new WalletPortifolioRequest { WalletHash = wallet.Hash });
                if (walletPortifolio?.Data?.Items != null)
                {
                    foreach (var item in walletPortifolio!.Data!.Items)
                    {
                        var token = (RecoverySaveTokenCommandResponse)null!;
                        if (item.Address == "So11111111111111111111111111111111111111112")
                            continue;
                        else if (item.Address == "So11111111111111111111111111111111111111111")
                            await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                        else
                            await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = item.Address });
                        await this._walletBalanceRepository.Add(new WalletBalance
                        {
                            IdWallet = wallet?.ID,
                            IdToken = token?.TokenId,
                            TokenHash = item.Address,
                            Quantity = item.UiAmount,
                            Price = item.PriceUsd,
                            TotalValueUSD = item.ValueUsd,
                            IsActive = item.UiAmount > 0,
                            LastUpdate = wallet?.DateLoadBalance
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
