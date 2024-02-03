using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.Triggers;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.Triggers
{
    public class VerifyAddTokenAlphaCommandHandler : IRequestHandler<VerifyAddTokenAlphaCommand, VerifyAddTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public VerifyAddTokenAlphaCommandHandler(IMediator mediator,
                                                 IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }
        public async Task<VerifyAddTokenAlphaCommandResponse> Handle(VerifyAddTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlphaCalled = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId);
            if (tokenAlphaCalled != null)
            {
                var tokenAlphaBuyBefore = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.WalletId == request.WalletId);
                if (tokenAlphaBuyBefore != null)
                {
                    tokenAlphaBuyBefore.ValueSpentSol += request?.ValueBuySol;
                    tokenAlphaBuyBefore.ValueSpentUSDC += request?.ValueBuyUSDC;
                    tokenAlphaBuyBefore.ValueSpentUSDT += request?.ValueBuyUSDT;
                    tokenAlphaBuyBefore.NumberOfBuys += 1;
                    await this._tokenAlphaWalletRepository.Edit(tokenAlphaBuyBefore);
                    await this._tokenAlphaWalletRepository.DetachedItem(tokenAlphaBuyBefore);
                }
                else 
                {
                    var tokekAlphaWallet = await this._tokenAlphaWalletRepository.Add(new TokenAlphaWallet 
                    {
                        TokenAlphaId = tokenAlphaCalled.ID,
                        WalletId = request.WalletId,
                        NumberOfBuys = 1,
                        ValueSpentSol = request?.ValueBuySol,
                        ValueSpentUSDC = request?.ValueBuyUSDC,
                        ValueSpentUSDT = request?.ValueBuyUSDT
                    });
                    await this._tokenAlphaWalletRepository.DetachedItem(tokekAlphaWallet);
                }
                tokenAlphaCalled.CallNumber += 1;
                tokenAlphaCalled.ActualMarketcap = request?.MarketCap;
                tokenAlphaCalled.ActualPrice = request?.Price;
                tokenAlphaCalled.IsCalledInChannel = false;
                tokenAlphaCalled.LastUpdate = DateTime.Now;
                await this._tokenAlphaRepository.Edit(tokenAlphaCalled);
                await this._tokenAlphaRepository.DetachedItem(tokenAlphaCalled);
            }
            else 
            {
                var maxDateOfLaunchDays = (decimal?)0;
                var buysBeforeThis = await this._walletBalanceHistoryRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId && x.Signature != request.Signature);
                if (buysBeforeThis == null) 
                {
                    if(request?.LaunchDate != null)
                        maxDateOfLaunchDays = this.CalculatedMaxDaysOfLaunch(request?.LaunchDate);
                    var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.MaxMarketcap < request!.MarketCap && x.MaxDateOfLaunchDays > maxDateOfLaunchDays, x => x.Ordernation!);
                    if (tokenAlphaConfiguration != null)
                    {
                        var tokenAlpha = await this._tokenAlphaRepository.Add(new TokenAlpha 
                        {
                            CallNumber = 1,
                            InitialMarketcap = request?.MarketCap,
                            ActualMarketcap = request?.MarketCap,
                            InitialPrice = request?.Price,
                            ActualPrice = request?.Price,
                            CreateDate = AdjustDateTimeToPtBR(request?.LaunchDate),
                            LastUpdate = null,
                            IsCalledInChannel = false,
                            TokenId = request?.TokenId,
                            TokenAlphaConfigurationId = tokenAlphaConfiguration.ID
                        });
                        await this._tokenAlphaRepository.DetachedItem(tokenAlpha);
                        var tokenAlphaWallet = await this._tokenAlphaWalletRepository.Add(new TokenAlphaWallet
                        {
                            TokenAlphaId = tokenAlpha.ID,
                            WalletId = request?.WalletId,
                            NumberOfBuys = 1,
                            ValueSpentSol = request?.ValueBuySol,
                            ValueSpentUSDC = request?.ValueBuyUSDC,
                            ValueSpentUSDT = request?.ValueBuyUSDT
                        });
                    }
                }
            }
            return new VerifyAddTokenAlphaCommandResponse { };
        }

        private decimal? CalculatedMaxDaysOfLaunch(DateTime? launchDate) 
        {
            var dateTimePtBr = this.AdjustDateTimeToPtBR(launchDate);
            var dateDiff = DateTime.Now - dateTimePtBr;
            return (decimal?)dateDiff?.TotalDays;
        }

        private DateTime? AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0);
        }
    }
}
