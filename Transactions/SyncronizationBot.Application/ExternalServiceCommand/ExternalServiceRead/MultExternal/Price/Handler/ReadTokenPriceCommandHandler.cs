using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Response;
using SyncronizationBot.Domain.Extensions;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Coingecko;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;
using DexScreener = SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token;
using Jupiter = SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices;
using CoinGecko = SyncronizationBot.Domain.Model.CrossCutting.Coingecko;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Handler
{
    public class ReadTokenPriceCommandHandler : IRequestHandler<ReadTokenPriceCommand, ReadTokenPriceCommandResponse>
    {
        private readonly IDexScreenerTokenService _dexScreenerTokenService;
        private readonly IJupiterPriceService _jupiterPriceService;
        private readonly ITokenOverviewService _tokensOverviewService;
        private readonly ICoingeckoTokenService _coingeckoTokenService;

        public ReadTokenPriceCommandHandler(IDexScreenerTokenService dexScreenerTokenService,
                                            IJupiterPriceService jupiterPriceService,
                                            ITokenOverviewService tokensOverviewService,
                                            ICoingeckoTokenService coingeckoTokenService)
        {
            this._dexScreenerTokenService = dexScreenerTokenService;
            this._jupiterPriceService = jupiterPriceService;
            this._tokensOverviewService = tokensOverviewService;
            this._coingeckoTokenService = coingeckoTokenService;
        }

        public async Task<ReadTokenPriceCommandResponse> Handle(ReadTokenPriceCommand request, CancellationToken cancellationToken)
        {
            var response  = await TryRecoveryPriceFromDexScreener(request);
            if (response.IsSuccess) return response;
            response = await TryRecoveryPriceFromJupiter(request);
            if (response.IsSuccess) return response;
            response = await TryRecoveryPriceFromBirdeye(request);
            if (response.IsSuccess) return response;
            response = await TryRecoveryPriceFromCoingecko(request);
            if (response.IsSuccess) return response;
            return new ReadTokenPriceCommandResponse { IsSuccess = false };
        }

        private async Task<ReadTokenPriceCommandResponse> TryRecoveryPriceFromJupiter(ReadTokenPriceCommand request)
        {
            var response = await this._jupiterPriceService.ExecuteRecoveryPriceAsync(new Jupiter.Request.JupiterPricesRequest { Ids = new List<string> { request.TokenHash } });
            if (response?.Data != null) 
            {
                if (response.Data.ContainsKey(request.TokenHash))
                    return new ReadTokenPriceCommandResponse 
                    { 
                        IsSuccess = true, 
                        PriceUsd = response.Data[request.TokenHash].Price,
                        Symbol = response.Data[request.TokenHash].MintSymbol,
                        FontPrice = EFontType.JUPITER 
                    };
            }
            return new ReadTokenPriceCommandResponse { IsSuccess = false };
        }

        private async Task<ReadTokenPriceCommandResponse> TryRecoveryPriceFromCoingecko(ReadTokenPriceCommand request)
        {
            var response = await this._coingeckoTokenService.ExecuteRecoveryTokenAsync(new CoinGecko.Request.TokenRequest { TokenHash = request.TokenHash });
            if (response != null) 
            {
                return new ReadTokenPriceCommandResponse
                {
                    Name = response.Name,
                    Symbol = response.Symbol,
                    NumberOfMarkets = response.Tickers?.Count,
                    PriceUsd = response?.MarketData?.CurrentPrice?.Usd,
                    PriceChange24 = response?.MarketData?.PriceChangePercentage24h,
                    Marketcap = response?.MarketData?.MarketCap?.Usd ?? response?.MarketData?.TotalSupply * response?.MarketData?.CurrentPrice?.Usd,
                    IsSuccess = true,
                    FontPrice = EFontType.COIN_GECKO
                };
            }
            return new ReadTokenPriceCommandResponse { IsSuccess = false };
        }

        private async Task<ReadTokenPriceCommandResponse> TryRecoveryPriceFromBirdeye(ReadTokenPriceCommand request)
        {
            var response = await _tokensOverviewService.ExecuteRecoveryTokenOverviewAsync(new TokenOverviewRequest { TokenHash = request.TokenHash });
            if (response?.Data != null) 
            {
                
                return new ReadTokenPriceCommandResponse
                {
                    Name = response.Data.Name,
                    Symbol = response.Data.Symbol,
                    NumberOfMarkets = response.Data.NumberMarkets,
                    PriceUsd = response.Data.Price,
                    PriceChange30m = response.Data.PriceChange30MPercent,
                    PriceChange4h = response.Data.PriceChange4HPercent,
                    PriceChange6h = response.Data.PriceChange6HPercent,
                    PriceChange24 = response.Data.PriceChange24HPercent,
                    Liquidity = response.Data.Liquidity,
                    Marketcap = response.Data.Mc ?? response.Data.Supply * response.Data.Price,
                    UniqueWallet24H = response.Data.UniqueWallet24H,
                    UniqueWalletHistory24H = response.Data.UniqueWalletHistory24H,
                    IsSuccess = true,
                    FontPrice = EFontType.BIRDEYE
                };
            }
            return new ReadTokenPriceCommandResponse { IsSuccess = false };
        }

        #region DexScreener rules

        private async Task<ReadTokenPriceCommandResponse> TryRecoveryPriceFromDexScreener(ReadTokenPriceCommand request)
        {
            try
            {
                var response = await this._dexScreenerTokenService.ExecuteRecoveryTokenAsync(new DexScreener.Request.TokenRequest { TokenHash = request.TokenHash });
                if (response?.Pairs?.Count > 0)
                {
                    var numberOfMarkets = response?.Pairs?.Select(p => p.DexId).Distinct().Count();
                    var pair = this.GetPairsFromAddress(response, "EPjFWdd5AufqSSqeM2qN1xzybapC8G4wEGGkZwyTDt1v");
                    pair = pair ?? this.GetPairsFromAddress(response, "Es9vMFrzaCERmJfrF4H2FYD4KCoNkY11McCe8BenwNYB");
                    pair = pair ?? this.GetPairsFromAddress(response, "So11111111111111111111111111111111111111112");
                    if (pair != null)
                    {
                        return new ReadTokenPriceCommandResponse
                        {
                            Name = pair?.BaseToken?.Name,
                            Symbol = pair?.BaseToken?.Symbol,
                            NumberOfMarkets = numberOfMarkets,
                            PriceUsd = pair?.PriceUsd?.ToDecimal(),
                            PriceChange5m = pair?.PriceChange?.M5.ToDecimal(),
                            PriceChange1h = pair?.PriceChange?.H1.ToDecimal(),
                            PriceChange6h = pair?.PriceChange?.H6.ToDecimal(),
                            PriceChange24 = pair?.PriceChange?.H24.ToDecimal(),
                            Liquidity = pair?.Liquidity?.Usd.ToDecimal(),
                            CreateDate = GetCreateDate(response),
                            Marketcap = pair?.Fdv?.ToDecimal(),
                            IsSuccess = true,
                            FontPrice = EFontType.DEXSCREENER
                        };
                    }
                }
            }
            finally { }
            return new ReadTokenPriceCommandResponse { FontPrice = EFontType.DEXSCREENER, IsSuccess = false };
        }

        private DexScreener.Response.Pair? GetPairsFromAddress(DexScreener.Response.TokenResponse? tokenResponse, string tokenHash) 
        {
            var pairs = tokenResponse?.Pairs?.FindAll(p => p.QuoteToken?.Address == tokenHash).ToList();
            if (pairs != null)
               return this.GetPriorityDexFromPair(pairs);
            return null;
        }

        private DexScreener.Response.Pair? GetPriorityDexFromPair(List<DexScreener.Response.Pair> pairs) 
        { 
            var pair = pairs.Find(x => x.DexId == "raydium");
            if (pair != null) return pair;
            pair = pairs.Find(x => x.DexId == "orca");
            if (pair != null) return pair;
            pair = pairs.Find(x => x.DexId == "meteora");
            if (pair != null) return pair;
            return pairs?.FirstOrDefault();
        }

        private DateTime GetCreateDate(DexScreener.Response.TokenResponse? response) 
        {
            var pair = this.GetPairsFromAddress(response, "So11111111111111111111111111111111111111112");
            if(pair?.PairCreatedAt != null) 
            {
                string pairCreateAt = pair!.PairCreatedAt!.ToString().Substring(0, 10);
                long.TryParse(pairCreateAt, out var longParsed);
                return DateTimeTicks.Instance.ConvertTicksToDateTime(longParsed);
            }
            return DateTime.MinValue;
        }

        #endregion
    }
}
