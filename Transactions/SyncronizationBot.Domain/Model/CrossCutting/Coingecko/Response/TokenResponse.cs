

using Newtonsoft.Json;

namespace SyncronizationBot.Domain.Model.CrossCutting.Coingecko.Response
{
    
    public class AthResponse
    {
        public decimal? Usd { get; set; }
    }

    public class AthChangePercentageResponse
    {
        public decimal? Usd { get; set; }
    }

    public class AthDateResponse
    {
        public DateTime? Brl { get; set; }
        public DateTime? Usd { get; set; }
    }

    public class AtlResponse
    {
        public decimal? Usd { get; set; }
    }

    public class AtlChangePercentageResponse
    {
        public decimal? Usd { get; set; }
    }

    public class AtlDateResponse
    {
        public DateTime? Brl { get; set; }
        public DateTime? Usd { get; set; }
    }

    public class CodeAdditionsDeletions4WeeksResponse
    {
        public object? Additions { get; set; }
        public object? Deletions { get; set; }
    }

    public class CommunityDataResponse
    {
        public object? FacebookLikes { get; set; }
        public int? TwitterFollowers { get; set; }
        public int? RedditAveragePosts48h { get; set; }
        public int? RedditAverageComments48h { get; set; }
        public int? RedditSubscribers { get; set; }
        public int? RedditAccountsActive48h { get; set; }
        public int? TelegramChannelUserCount { get; set; }
    }

    public class ConvertedLastResponse
    {
        public decimal? Btc { get; set; }
        public decimal? Eth { get; set; }
        public decimal? Usd { get; set; }
    }

    public class ConvertedVolumeResponse
    {
        public decimal? Btc { get; set; }
        public decimal? Eth { get; set; }
        public decimal? Usd { get; set; }
    }

    public class CurrentPriceResponse
    {
        public decimal? Usd { get; set; }
    }

    public class DescriptionResponse
    {
        public string? En { get; set; }
    }

    public class DetailPlatformsResponse
    {
        public SolanaResponse? Solana { get; set; }
    }

    public class DeveloperDataResponse
    {
        public int? Forks { get; set; }
        public int? Stars { get; set; }
        public int? Subscribers { get; set; }
        public int? TotalIssues { get; set; }
        public int? ClosedIssues { get; set; }
        public int? PullRequestsMerged { get; set; }
        public int? PullRequestContributors { get; set; }
        public CodeAdditionsDeletions4WeeksResponse? CodeAdditionsDeletions4Weeks { get; set; }
        public int? CommitCount4Weeks { get; set; }
        public List<object?>? Last4WeeksCommitActivitySeries { get; set; }
    }

    public class FullyDilutedValuationResponse
    {
        public int? Usd { get; set; }
    }

    public class High24hResponse
    {
        public decimal? Usd { get; set; }
    }

    public class ImageResponse
    {
        public string? Thumb { get; set; }
        public string? Small { get; set; }
        public string? Large { get; set; }
    }

    public class LinksResponse
    {
        public List<string?>? Homepage { get; set; }
        public string? Whitepaper { get; set; }
        public List<string?>? BlockchainSite { get; set; }
        public List<string?>? OfficialForumUrl { get; set; }
        public List<string?>? ChatUrl { get; set; }
        public List<string?>? AnnouncementUrl { get; set; }
        public string? TwitterScreenName { get; set; }
        public string? FacebookUsername { get; set; }
        public object? BitcointalkThreadIdentifier { get; set; }
        public string? TelegramChannelIdentifier { get; set; }
        public string? SubredditUrl { get; set; }
        public ReposUrlResponse? ReposUrl { get; set; }
    }

    public class LocalizationResponse
    {
        public string? En { get; set; }
    }

    public class Low24hResponse
    {
        public decimal? Usd { get; set; }
    }

    public class MarketResponse
    {
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public bool? HasTradingIncentive { get; set; }
    }

    public class MarketCapResponse
    {
        public int? Usd { get; set; }
    }

    public class MarketCapChange24hInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class MarketCapChangePercentage24hInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class MarketDataResponse
    {
        public CurrentPriceResponse? CurrentPrice { get; set; }
        public decimal? TotalValueLocked { get; set; }
        public decimal? McapToTvlRatio { get; set; }
        public decimal? FdvToTvlRatio { get; set; }
        public decimal? Roi { get; set; }
        public AthResponse? Ath { get; set; }
        public AthChangePercentageResponse? AthChangePercentage { get; set; }
        public AthDateResponse? AthDate { get; set; }
        public AtlResponse? Atl { get; set; }
        public AtlChangePercentageResponse? AtlChangePercentage { get; set; }
        public AtlDateResponse? AtlDate { get; set; }
        public MarketCapResponse? MarketCap { get; set; }
        public int? MarketCapRank { get; set; }
        public FullyDilutedValuationResponse? FullyDilutedValuation { get; set; }
        public int? MarketCapFdvRatio { get; set; }
        public TotalVolumeResponse? TotalVolume { get; set; }
        public High24hResponse? High24h { get; set; }
        public Low24hResponse? Low24h { get; set; }
        public decimal? PriceChange24h { get; set; }
        public decimal? PriceChangePercentage24h { get; set; }
        public decimal? PriceChangePercentage7d { get; set; }
        public decimal? PriceChangePercentage14d { get; set; }
        public decimal? PriceChangePercentage30d { get; set; }
        public decimal? PriceChangePercentage60d { get; set; }
        public int? PriceChangePercentage200d { get; set; }
        public int? PriceChangePercentage1y { get; set; }
        public decimal? MarketCapChange24h { get; set; }
        public decimal? MarketCapChangePercentage24h { get; set; }
        public PriceChange24hInCurrencyResponse? PriceChange24hInCurrency { get; set; }
        public PriceChangePercentage1hInCurrencyResponse? PriceChangePercentage1hInCurrency { get; set; }
        public PriceChangePercentage24hInCurrencyResponse? PriceChangePercentage24hInCurrency { get; set; }
        public PriceChangePercentage7dInCurrencyResponse? PriceChangePercentage7dInCurrency { get; set; }
        public PriceChangePercentage14dInCurrencyResponse? PriceChangePercentage14dInCurrency { get; set; }
        public PriceChangePercentage30dInCurrencyResponse? PriceChangePercentage30dInCurrency { get; set; }
        public PriceChangePercentage60dInCurrencyResponse? PriceChangePercentage60dInCurrency { get; set; }
        public PriceChangePercentage200dInCurrencyResponse? PriceChangePercentage200dInCurrency { get; set; }
        public PriceChangePercentage1yInCurrencyResponse? PriceChangePercentage1yInCurrency { get; set; }
        public MarketCapChange24hInCurrencyResponse? MarketCapChange24hInCurrency { get; set; }
        public MarketCapChangePercentage24hInCurrencyResponse? MarketCapChangePercentage24hInCurrency { get; set; }
        public int? TotalSupply { get; set; }
        public int? MaxSupply { get; set; }
        public int? CirculatingSupply { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class PlatformsResponse
    {
        public string? Solana { get; set; }
    }

    public class PriceChange24hInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage14dInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage1hInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage1yInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage200dInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage24hInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage30dInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage60dInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class PriceChangePercentage7dInCurrencyResponse
    {
        public decimal? Usd { get; set; }
    }

    public class ReposUrlResponse
    {
        public List<object?>? Github { get; set; }
        public List<object?>? Bitbucket { get; set; }
    }

    public class TokenResponse
    {
        public string? Id { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public string? WebSlug { get; set; }
        public string? AssetPlatformId { get; set; }
        public PlatformsResponse? Platforms { get; set; }
        public DetailPlatformsResponse? DetailPlatforms { get; set; }
        public int? BlockTimeInMinutes { get; set; }
        public object? HashingAlgorithm { get; set; }
        public List<string?>? Categories { get; set; }
        public bool? PreviewListing { get; set; }
        public object? PublicNotice { get; set; }
        public List<object?>? AdditionalNotices { get; set; }
        public LocalizationResponse? Localization { get; set; }
        public DescriptionResponse? Description { get; set; }
        public LinksResponse? Links { get; set; }
        public ImageResponse? Image { get; set; }
        public string? CountryOrigin { get; set; }
        public object? GenesisDate { get; set; }
        public string? ContractAddress { get; set; }
        public int? SentimentVotesUpPercentage { get; set; }
        public int? SentimentVotesDownPercentage { get; set; }
        public int? WatchlistPortfolioUsers { get; set; }
        public int? MarketCapRank { get; set; }
        public MarketDataResponse? MarketData { get; set; }
        public CommunityDataResponse? CommunityData { get; set; }
        public DeveloperDataResponse? DeveloperData { get; set; }
        public List<object?>? StatusUpdates { get; set; }
        public DateTime? LastUpdated { get; set; }
        public List<TickerResponse>? Tickers { get; set; }
    }

    public class SolanaResponse
    {
        public int? DecimalPlace { get; set; }
        public string? ContractAddress { get; set; }
    }

    public class TickerResponse
    {
        public string? Base { get; set; }
        public string? Target { get; set; }
        public MarketResponse? Market { get; set; }
        public decimal? Last { get; set; }
        public decimal? Volume { get; set; }
        public ConvertedLastResponse? ConvertedLast { get; set; }
        public ConvertedVolumeResponse? ConvertedVolume { get; set; }
        public string? TrustScore { get; set; }
        public decimal? BidAskSpreadPercentage { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? LastTradedAt { get; set; }
        public DateTime? LastFetchAt { get; set; }
        public bool? IsAnomaly { get; set; }
        public bool? IsStale { get; set; }
        public string? TradeUrl { get; set; }
        public object? TokenInfoUrl { get; set; }
        public string? CoinId { get; set; }
        public string? TargetCoinId { get; set; }
    }

    public class TotalVolumeResponse
    {
        public int? Usd { get; set; }
    }
}
