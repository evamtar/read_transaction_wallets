using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.Birdeye;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.Birdeye;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.Birdeye
{
    public class RecoverySaveBalanceBirdeyeCommandHandler : BaseWalletHandler, IRequestHandler<RecoverySaveBalanceBirdeyeCommand, RecoverySaveBalanceBirdeyeCommandResponse>
    {
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IWalletPortifolioService _walletPortifolioService;
        private readonly IAccountInfoService _accountInfoService;
        public RecoverySaveBalanceBirdeyeCommandHandler(IMediator mediator,
                                                        IWalletRepository walletRepository,
                                                        IOptions<SyncronizationBotConfig> config,
                                                        IWalletBalanceRepository walletBalanceRepository,
                                                        IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                        IWalletPortifolioService walletPortifolioService,
                                                        IAccountInfoService accountInfoService) : base(mediator, walletRepository, EFontType.BIRDEYE, config)
        {
            this._walletBalanceRepository = walletBalanceRepository;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._walletPortifolioService = walletPortifolioService;
            this._accountInfoService = accountInfoService;
        }

        public async Task<RecoverySaveBalanceBirdeyeCommandResponse> Handle(RecoverySaveBalanceBirdeyeCommand request, CancellationToken cancellationToken)
        {
            var token = (RecoverySaveTokenCommandResponse)null!;
            var walletPortifolio = await this._walletPortifolioService.ExecuteRecoveryWalletPortifolioAsync(new WalletPortifolioRequest { WalletHash = request.WalletHash });
            var dateLoadBalance = DateTime.Now;
            if (walletPortifolio?.Data?.Items != null)
            {
                foreach (var item in walletPortifolio!.Data!.Items)
                {
                    if (item.Address == "So11111111111111111111111111111111111111111" || item.Address == "So11111111111111111111111111111111111111112")
                        token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112", LazyLoad = true });
                    else 
                        token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = item.Address, LazyLoad = true });
                    await this.SaveBalance(request, token, item, dateLoadBalance);
                }
            }
            return new RecoverySaveBalanceBirdeyeCommandResponse { DateLoadBalance = (base.IsSaveBalance() ?? false) ? dateLoadBalance : null };
        }

        private async Task SaveBalance(RecoverySaveBalanceBirdeyeCommand request, RecoverySaveTokenCommandResponse token, ResultItem item, DateTime? dateLoadBalance) 
        {
            var balance = (WalletBalance)null!;
            if (base.IsSaveBalance() ?? false)
            {
                balance = await this._walletBalanceRepository.AddAsync(new WalletBalance
                {
                    WalletId = request.WalletId,
                    TokenId = token?.TokenId,
                    TokenHash = item.Address,
                    Quantity = item.UiAmount,
                    Price = item.PriceUsd,
                    TotalValueUSD = item.ValueUsd,
                    IsActive = item.UiAmount > 0,
                    LastUpdate = dateLoadBalance
                });
                await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                {
                    WalletBalanceId = balance?.ID,
                    WalletId = balance?.WalletId,
                    TokenId = balance?.TokenId,
                    TokenHash = balance?.TokenHash,
                    OldQuantity = (decimal?)0,
                    NewQuantity = balance?.Quantity,
                    RequestQuantity = balance?.Quantity,
                    PercentageCalculated = 100,
                    Price = balance?.Price,
                    TotalValueUSD = balance?.TotalValueUSD,
                    Signature = "CREATE BALANCE",
                    CreateDate = DateTime.Now,
                    LastUpdate = balance?.LastUpdate
                });
            }
            else
            {
                await this._walletBalanceHistoryRepository.AddAsync(new WalletBalanceHistory
                {
                    WalletBalanceId = null,
                    WalletId = request.WalletId,
                    TokenId = token?.TokenId,
                    TokenHash = item.Address,
                    OldQuantity = (decimal?)0,
                    NewQuantity = item.UiAmount,
                    RequestQuantity = item.UiAmount,
                    PercentageCalculated = 100,
                    Price = item.PriceUsd,
                    TotalValueUSD = item.ValueUsd,
                    Signature = "CREATE BALANCE",
                    CreateDate = DateTime.Now,
                    LastUpdate = dateLoadBalance
                });
            }
        }
    }
}
