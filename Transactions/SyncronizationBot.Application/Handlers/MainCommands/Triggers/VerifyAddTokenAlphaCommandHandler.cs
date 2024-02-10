using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.Triggers;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.Triggers
{
    public class VerifyAddTokenAlphaCommandHandler : IRequestHandler<VerifyAddTokenAlphaCommand, VerifyAddTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaHistoryRepository _tokenAlphaHistoryRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly ITokenAlphaWalletHistoryRepository _tokenAlphaWalletHistoryRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IPublishMessageRepository _publishMessageRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public VerifyAddTokenAlphaCommandHandler(IMediator mediator,
                                                 IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaHistoryRepository tokenAlphaHistoryRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                                 ITokenAlphaWalletHistoryRepository tokenAlphaWalletHistoryRepository,
                                                 IPublishMessageRepository publishMessageRepository,
                                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaHistoryRepository = tokenAlphaHistoryRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._tokenAlphaWalletHistoryRepository = tokenAlphaWalletHistoryRepository;
            this._publishMessageRepository = publishMessageRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }
        public async Task<VerifyAddTokenAlphaCommandResponse> Handle(VerifyAddTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlphaCalled = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId);
            if (tokenAlphaCalled != null)
            {
                var tokenAlphaBuyBefore = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlphaCalled.ID && x.WalletId == request.WalletId);
                if (tokenAlphaBuyBefore != null)
                {
                    tokenAlphaBuyBefore.ValueSpentSol += request?.ValueBuySol;
                    tokenAlphaBuyBefore.ValueSpentUSDC += request?.ValueBuyUSDC;
                    tokenAlphaBuyBefore.ValueSpentUSDT += request?.ValueBuyUSDT;
                    tokenAlphaBuyBefore.QuantityToken += request?.QuantityTokenReceived;
                    await this._tokenAlphaWalletRepository.Edit(tokenAlphaBuyBefore);
                    await this._tokenAlphaWalletRepository.DetachedItem(tokenAlphaBuyBefore);
                    await SaveTokenAlphaWalletsHistory(request, tokenAlphaBuyBefore);
                }
                else 
                {
                    var tokekAlphaWallet = await this._tokenAlphaWalletRepository.Add(new TokenAlphaWallet 
                    {
                        TokenAlphaId = tokenAlphaCalled.ID,
                        WalletId = request.WalletId,
                        WalletHash = request?.WalletHash,
                        ClassWalletDescription = request?.ClassWalletDescription,
                        NumberOfBuys = 1,
                        ValueSpentSol = request?.ValueBuySol,
                        ValueSpentUSDC = request?.ValueBuyUSDC,
                        ValueSpentUSDT = request?.ValueBuyUSDT,
                        QuantityToken = request?.QuantityTokenReceived
                    });
                    await this._tokenAlphaWalletRepository.DetachedItem(tokekAlphaWallet);
                    await SaveTokenAlphaWalletsHistory(request, tokekAlphaWallet);
                }
                tokenAlphaCalled.CallNumber += 1;
                tokenAlphaCalled.ActualMarketcap = request?.MarketCap;
                tokenAlphaCalled.ActualPrice = request?.Price;
                tokenAlphaCalled.TokenHash = request?.TokenHash;
                tokenAlphaCalled.TokenName = request?.TokenName;
                tokenAlphaCalled.TokenSymbol = request?.TokenSymbol;
                tokenAlphaCalled.LastUpdate = DateTime.Now;
                await this._tokenAlphaRepository.Edit(tokenAlphaCalled);
                await this._tokenAlphaRepository.DetachedItem(tokenAlphaCalled);
                await SaveTokenAlphaHistory(request, tokenAlphaCalled);
                var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.ID == tokenAlphaCalled.TokenAlphaConfigurationId);
                await PublishMessage(tokenAlphaCalled, tokenAlphaConfiguration!);
                await this._tokenAlphaConfigurationRepository.DetachedItem(tokenAlphaConfiguration!);
            }
            else 
            {
                var buysBeforeThis = await this._walletBalanceHistoryRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId && x.Signature != request.Signature);
                if (buysBeforeThis == null) 
                {
                    var tokenAlphaConfiguration = await this.GetTokenAlphaConfiguration(request, 0);
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
                            TokenId = request?.TokenId,
                            TokenHash = request?.TokenHash,
                            TokenName = request?.TokenName,
                            TokenSymbol = request?.TokenSymbol,
                            TokenAlphaConfigurationId = tokenAlphaConfiguration.ID
                        });
                        await this._tokenAlphaRepository.DetachedItem(tokenAlpha);
                        await SaveTokenAlphaHistory(request, tokenAlpha);

                        var tokenAlphaWallet = await this._tokenAlphaWalletRepository.Add(new TokenAlphaWallet
                        {
                            TokenAlphaId = tokenAlpha.ID,
                            WalletId = request?.WalletId,
                            WalletHash = request?.WalletHash,
                            ClassWalletDescription = request?.ClassWalletDescription,
                            NumberOfBuys = 1,
                            ValueSpentSol = request?.ValueBuySol,
                            ValueSpentUSDC = request?.ValueBuyUSDC,
                            ValueSpentUSDT = request?.ValueBuyUSDT,
                            QuantityToken = request?.QuantityTokenReceived
                        });
                        await SaveTokenAlphaWalletsHistory(request, tokenAlphaWallet);
                        await PublishMessage(tokenAlpha, tokenAlphaWallet, tokenAlphaConfiguration);
                    }
                }
            }
            
            return new VerifyAddTokenAlphaCommandResponse { };
        }

        private async Task SaveTokenAlphaHistory(VerifyAddTokenAlphaCommand? request, TokenAlpha? tokenAlpha) 
        {
            var tokenAlphaHistory = await this._tokenAlphaHistoryRepository.Add(new TokenAlphaHistory
            {
                TokenAlphaId = tokenAlpha?.ID,
                CallNumber = tokenAlpha?.CallNumber,
                InitialMarketcap = tokenAlpha?.InitialMarketcap,
                ActualMarketcap = tokenAlpha?.ActualMarketcap,
                InitialPrice = tokenAlpha?.InitialPrice,
                ActualPrice = tokenAlpha?.ActualPrice,
                RequestMarketCap = request?.MarketCap,
                RequestPrice = request?.Price,
                CreateDate = tokenAlpha?.CreateDate,
                LastUpdate = tokenAlpha?.LastUpdate,
                TokenId = tokenAlpha?.TokenId,
                TokenHash = tokenAlpha?.TokenHash,
                TokenName = tokenAlpha?.TokenName,
                TokenSymbol = tokenAlpha?.TokenSymbol,
                TokenAlphaConfigurationId = tokenAlpha?.TokenAlphaConfigurationId
            });
            await this._tokenAlphaHistoryRepository.DetachedItem(tokenAlphaHistory);
        }

        private async Task SaveTokenAlphaWalletsHistory(VerifyAddTokenAlphaCommand? request, TokenAlphaWallet? tokenAlphaWallet)
        {
            var tokenAlphaWalletHistory = await this._tokenAlphaWalletHistoryRepository.Add(new TokenAlphaWalletHistory
            {
                TokenAlphaWalletId = tokenAlphaWallet?.ID,
                TokenAlphaId = tokenAlphaWallet?.TokenAlphaId,
                WalletId = tokenAlphaWallet?.WalletId,
                WalletHash = tokenAlphaWallet?.WalletHash,
                ClassWalletDescription = tokenAlphaWallet?.ClassWalletDescription,
                NumberOfBuys = tokenAlphaWallet?.NumberOfBuys,
                ValueSpentSol = tokenAlphaWallet?.ValueSpentSol,
                ValueSpentUSDC = tokenAlphaWallet?.ValueSpentUSDC,
                ValueSpentUSDT = tokenAlphaWallet?.ValueSpentUSDT,
                QuantityToken = tokenAlphaWallet?.QuantityToken,
                NumberOfSells = tokenAlphaWallet?.NumberOfSells,
                ValueReceivedSol = tokenAlphaWallet?.ValueReceivedSol,
                ValueReceivedUSDC = tokenAlphaWallet?.ValueReceivedUSDC,
                ValueReceivedUSDT = tokenAlphaWallet?.ValueReceivedUSDT,
                QuantityTokenSell = tokenAlphaWallet?.QuantityTokenSell,
                RequestValueInSol = request?.ValueBuySol,
                RequestValueInUSDC = request?.ValueBuyUSDC,
                RequestValueInUSDT = request?.ValueBuyUSDT,
                RequestQuantityToken = request?.QuantityTokenReceived
            });
            await this._tokenAlphaWalletHistoryRepository.DetachedItem(tokenAlphaWalletHistory);
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

        private async Task<TokenAlphaConfiguration> GetTokenAlphaConfiguration(VerifyAddTokenAlphaCommand? request, int? ordenation) 
        {
            var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.Ordernation >= ordenation, x => x.Ordernation!);

            if(tokenAlphaConfiguration != null) 
            {
                var maxDateOfLaunchDays = this.CalculatedMaxDaysOfLaunch(request?.LaunchDate);
                if (request?.MarketCap <= tokenAlphaConfiguration.MaxMarketcap  && maxDateOfLaunchDays <= tokenAlphaConfiguration.MaxDateOfLaunchDays)
                    return tokenAlphaConfiguration;
                else 
                    return await this.GetTokenAlphaConfiguration(request, tokenAlphaConfiguration.Ordernation + 1);
                
            }
            return null!;
        }

        private async Task PublishMessage(TokenAlpha tokenAlpha, TokenAlphaConfiguration tokenAlphaConfiguration) 
        { 
            var listTokenAlphaWalletsIds = new List<Guid?>();
            var tokenAlphaWallet = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlpha.ID);
            var hasNext = tokenAlphaWallet != null;
            var publishMessageAlpha = await this.SavePublishMessage(tokenAlpha, null);
            await this.SavePublishMessage(tokenAlphaConfiguration, publishMessageAlpha.ID);
            while (hasNext) 
            {
                listTokenAlphaWalletsIds.Add(tokenAlphaWallet!.ID);
                await this.SavePublishMessage(tokenAlphaWallet, publishMessageAlpha.ID);
                tokenAlphaWallet = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlpha.ID && !listTokenAlphaWalletsIds.Contains(x.ID));
                hasNext = tokenAlphaWallet != null;
            }
        }

        private async Task PublishMessage(TokenAlpha tokenAlpha, TokenAlphaWallet tokenAlphaWallet, TokenAlphaConfiguration tokenAlphaConfiguration)
        {
            var publishMessageAlpha = await this.SavePublishMessage(tokenAlpha, null);
            await this.SavePublishMessage(tokenAlphaConfiguration, publishMessageAlpha.ID);
            await this.SavePublishMessage(tokenAlphaWallet, publishMessageAlpha.ID);
        }

        private async Task<PublishMessage> SavePublishMessage<T>(T entity, Guid? parentId) where T : Entity 
        {
            var publishMessage = await this._publishMessageRepository.Add(new PublishMessage
            {
                EntityId = entity.ID,
                Entity = typeof(T).ToString(),
                JsonValue = entity.JsonSerialize(),
                ItWasPublished = false,
                EntityParentId = parentId,
            });
            await this._publishMessageRepository.DetachedItem(publishMessage);
            return publishMessage;
        }

    }
}
