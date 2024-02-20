using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.Triggers;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Repository.SQLServer;

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
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public VerifyAddTokenAlphaCommandHandler(IMediator mediator,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaHistoryRepository tokenAlphaHistoryRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                                 ITokenAlphaWalletHistoryRepository tokenAlphaWalletHistoryRepository,
                                                 IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                 ITransactionsRepository transactionsRepository,
                                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaHistoryRepository = tokenAlphaHistoryRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._tokenAlphaWalletHistoryRepository = tokenAlphaWalletHistoryRepository;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._transactionsRepository = transactionsRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }
        public async Task<VerifyAddTokenAlphaCommandResponse> Handle(VerifyAddTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlphaCalled = await this._tokenAlphaRepository.FindFirstOrDefaultAsync(x => x.TokenId == request.TokenId);
            if (tokenAlphaCalled != null)
            {
                var tokenAlphaBuyBefore = await this._tokenAlphaWalletRepository.FindFirstOrDefaultAsync(x => x.TokenAlphaId == tokenAlphaCalled.ID && x.WalletId == request.WalletId);
                if (tokenAlphaBuyBefore != null)
                {
                    tokenAlphaBuyBefore.ValueSpentSol += request?.ValueBuySol;
                    tokenAlphaBuyBefore.ValueSpentUSD += request?.ValueBuyUSD;
                    tokenAlphaBuyBefore.QuantityToken += request?.QuantityTokenReceived;
                    this._tokenAlphaWalletRepository.Update(tokenAlphaBuyBefore);
                    await this._tokenAlphaWalletRepository.DetachedItemAsync(tokenAlphaBuyBefore);
                    await SaveTokenAlphaWalletsHistory(request, tokenAlphaBuyBefore);
                }
                else 
                {
                    var tokekAlphaWallet = await this._tokenAlphaWalletRepository.AddAsync(new TokenAlphaWallet 
                    {
                        TokenAlphaId = tokenAlphaCalled.ID,
                        WalletId = request.WalletId,
                        WalletHash = request?.WalletHash,
                        ClassWalletDescription = request?.ClassWalletDescription,
                        NumberOfBuys = 1,
                        ValueSpentSol = request?.ValueBuySol,
                        ValueSpentUSD = request?.ValueBuyUSD,
                        QuantityToken = request?.QuantityTokenReceived
                    });
                    await this._tokenAlphaWalletRepository.DetachedItemAsync(tokekAlphaWallet);
                    await SaveTokenAlphaWalletsHistory(request, tokekAlphaWallet);
                }
                tokenAlphaCalled.CallNumber += 1;
                tokenAlphaCalled.ActualMarketcap = request?.MarketCap;
                tokenAlphaCalled.ActualPrice = request?.Price;
                tokenAlphaCalled.TokenHash = request?.TokenHash;
                tokenAlphaCalled.TokenName = request?.TokenName;
                tokenAlphaCalled.TokenSymbol = request?.TokenSymbol;
                tokenAlphaCalled.LastUpdate = DateTime.Now;
                this._tokenAlphaRepository.Update(tokenAlphaCalled);
                await this._tokenAlphaRepository.DetachedItemAsync(tokenAlphaCalled);
                await SaveTokenAlphaHistory(request, tokenAlphaCalled);
                var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefaultAsync(x => x.ID == tokenAlphaCalled.TokenAlphaConfigurationId);
                await PublishMessage(tokenAlphaCalled, tokenAlphaConfiguration!);
                await this._tokenAlphaConfigurationRepository.DetachedItemAsync(tokenAlphaConfiguration!);
            }
            else 
            {
                var buysBeforeThis = await this._walletBalanceHistoryRepository.FindFirstOrDefaultAsync(x => x.TokenId == request.TokenId && x.Signature != request.Signature);
                var transactionsBefore = (Transactions?)null!;
                ///TODO:EVANDRO
                //var transactionsBefore = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature != request.Signature && (x.TokenSourceId == request.TokenId || x.TokenDestinationId == request.TokenId));
                if (transactionsBefore == null && buysBeforeThis == null) 
                {
                    var tokenAlphaConfiguration = await this.GetTokenAlphaConfiguration(request);
                    if (tokenAlphaConfiguration != null)
                    {
                        var tokenAlpha = await this._tokenAlphaRepository.AddAsync(new TokenAlpha
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
                        await this._tokenAlphaRepository.DetachedItemAsync(tokenAlpha);
                        await SaveTokenAlphaHistory(request, tokenAlpha);

                        var tokenAlphaWallet = await this._tokenAlphaWalletRepository.AddAsync(new TokenAlphaWallet
                        {
                            TokenAlphaId = tokenAlpha.ID,
                            WalletId = request?.WalletId,
                            WalletHash = request?.WalletHash,
                            ClassWalletDescription = request?.ClassWalletDescription,
                            NumberOfBuys = 1,
                            ValueSpentSol = request?.ValueBuySol,
                            ValueSpentUSD = request?.ValueBuyUSD,
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
            var tokenAlphaHistory = await this._tokenAlphaHistoryRepository.AddAsync(new TokenAlphaHistory
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
            await this._tokenAlphaHistoryRepository.DetachedItemAsync(tokenAlphaHistory);
        }

        private async Task SaveTokenAlphaWalletsHistory(VerifyAddTokenAlphaCommand? request, TokenAlphaWallet? tokenAlphaWallet)
        {
            var tokenAlphaWalletHistory = await this._tokenAlphaWalletHistoryRepository.AddAsync(new TokenAlphaWalletHistory
            {
                TokenAlphaWalletId = tokenAlphaWallet?.ID,
                TokenAlphaId = tokenAlphaWallet?.TokenAlphaId,
                WalletId = tokenAlphaWallet?.WalletId,
                WalletHash = tokenAlphaWallet?.WalletHash,
                ClassWalletDescription = tokenAlphaWallet?.ClassWalletDescription,
                NumberOfBuys = tokenAlphaWallet?.NumberOfBuys,
                ValueSpentSol = tokenAlphaWallet?.ValueSpentSol,
                ValueSpentUSD = tokenAlphaWallet?.ValueSpentUSD,
                QuantityToken = tokenAlphaWallet?.QuantityToken,
                NumberOfSells = tokenAlphaWallet?.NumberOfSells,
                ValueReceivedSol = tokenAlphaWallet?.ValueReceivedSol,
                ValueReceivedUSD = tokenAlphaWallet?.ValueReceivedUSD,
                QuantityTokenSell = tokenAlphaWallet?.QuantityTokenSell,
                RequestValueInSol = request?.ValueBuySol,
                RequestValueInUSD = request?.ValueBuyUSD,
                RequestQuantityToken = request?.QuantityTokenReceived
            });
            await this._tokenAlphaWalletHistoryRepository.DetachedItemAsync(tokenAlphaWalletHistory);
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

        private async Task<TokenAlphaConfiguration> GetTokenAlphaConfiguration(VerifyAddTokenAlphaCommand? request) 
        {
            var tokenAlphaConfigurations = await this._tokenAlphaConfigurationRepository.GetAsync(x => x.Ordernation > -1, x => x.Ordernation!);
            if(tokenAlphaConfigurations.Count > 0) 
            {
                foreach (var tokenAlphaConfiguration in tokenAlphaConfigurations)
                {
                    var maxDateOfLaunchDays = this.CalculatedMaxDaysOfLaunch(request?.LaunchDate);
                    if (request?.MarketCap <= tokenAlphaConfiguration.MaxMarketcap && maxDateOfLaunchDays <= tokenAlphaConfiguration.MaxDateOfLaunchDays)
                        return tokenAlphaConfiguration;
                }
            }
            return null!;
        }

        private async Task PublishMessage(TokenAlpha tokenAlpha, TokenAlphaConfiguration tokenAlphaConfiguration) 
        { 
            //var tokenAlphaWallets = await this._tokenAlphaWalletRepository.GetAsync(x => x.TokenAlphaId == tokenAlpha.ID);
            //var publishMessageAlpha = await this.SavePublishMessage(tokenAlpha, null);
            //await this.SavePublishMessage(tokenAlphaConfiguration, publishMessageAlpha.ID);
            //if (tokenAlphaWallets?.Count > 0) 
            //{
            //    foreach (var tokenAlphaWallet in tokenAlphaWallets)
            //        await this.SavePublishMessage(tokenAlphaWallet, publishMessageAlpha.ID);
            //}
        }

        private async Task PublishMessage(TokenAlpha tokenAlpha, TokenAlphaWallet tokenAlphaWallet, TokenAlphaConfiguration tokenAlphaConfiguration)
        {
            //var publishMessageAlpha = await this.SavePublishMessage(tokenAlpha, null);
            //await this.SavePublishMessage(tokenAlphaConfiguration, publishMessageAlpha.ID);
            //await this.SavePublishMessage(tokenAlphaWallet, publishMessageAlpha.ID);
        }

        //private async Task<PublishMessage> SavePublishMessage<T>(T entity, Guid? parentId) where T : Entity 
        //{
        //    var publishMessage = await this._publishMessageRepository.AddAsync(new PublishMessage
        //    {
        //        EntityId = entity.ID,
        //        Entity = typeof(T).ToString(),
        //        JsonValue = entity.JsonSerialize(),
        //        ItWasPublished = false,
        //        EntityParentId = parentId,
        //    });
        //    await this._publishMessageRepository.DetachedItemAsync(publishMessage);
        //    return publishMessage;
        //}

    }
}
