

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        [JsonProperty("facebook_likes")]
        public object? FacebookLikes { get; set; }
        [JsonProperty("twitter_followers")]
        public decimal? TwitterFollowers { get; set; }
        [JsonProperty("reddit_average_posts_48h")]
        public decimal? RedditAveragePosts48h { get; set; }
        [JsonProperty("reddit_average_comments_48h")]
        public decimal? RedditAverageComments48h { get; set; }
        [JsonProperty("reddit_subscribers")]
        public decimal? RedditSubscribers { get; set; }
        [JsonProperty("reddit_accounts_active_48h")]
        public decimal? RedditAccountsActive48h { get; set; }
        [JsonProperty("telegram_channel_user_count")]
        public decimal? TelegramChannelUserCount { get; set; }
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
        public decimal? Forks { get; set; }
        public decimal? Stars { get; set; }
        public decimal? Subscribers { get; set; }
        [JsonProperty("total_issues")]
        public decimal? TotalIssues { get; set; }
        [JsonProperty("closed_issues")]
        public decimal? ClosedIssues { get; set; }
        [JsonProperty("pull_requests_merged")]
        public decimal? PullRequestsMerged { get; set; }
        [JsonProperty("pull_request_contributors")]
        public decimal? PullRequestContributors { get; set; }
        [JsonProperty("code_additions_deletions_4_weeks")]
        public CodeAdditionsDeletions4WeeksResponse? CodeAdditionsDeletions4Weeks { get; set; }
        [JsonProperty("commit_count_4_weeks")]
        public decimal? CommitCount4Weeks { get; set; }
        [JsonProperty("facebook_lilast_4_weeks_commit_activity_serieskes")]
        public List<object?>? Last4WeeksCommitActivitySeries { get; set; }
    }

    public class FullyDilutedValuationResponse
    {
        public decimal? Usd { get; set; }
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
        [JsonProperty("blockchain_site")]
        public List<string?>? BlockchainSite { get; set; }
        [JsonProperty("official_forum_url")]
        public List<string?>? OfficialForumUrl { get; set; }
        [JsonProperty("chat_url")]
        public List<string?>? ChatUrl { get; set; }
        [JsonProperty("announcement_url")]
        public List<string?>? AnnouncementUrl { get; set; }
        [JsonProperty("twitter_screen_name")]
        public string? TwitterScreenName { get; set; }
        [JsonProperty("facebook_username")]
        public string? FacebookUsername { get; set; }
        [JsonProperty("bitcointalk_thread_identifier")]
        public object? BitcointalkThreadIdentifier { get; set; }
        [JsonProperty("telegram_channel_identifier")]
        public string? TelegramChannelIdentifier { get; set; }
        [JsonProperty("subreddit_url")]
        public string? SubredditUrl { get; set; }
        [JsonProperty("repos_url")]
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
        [JsonProperty("has_trading_incentive")]
        public bool? HasTradingIncentive { get; set; }
    }

    public class MarketCapResponse
    {
        public decimal? Usd { get; set; }
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
        [JsonProperty("current_price")]
        public CurrentPriceResponse? CurrentPrice { get; set; }
        [JsonProperty("total_value_locked")]
        public decimal? TotalValueLocked { get; set; }
        [JsonProperty("mcap_to_tvl_ratio")]
        public decimal? McapToTvlRatio { get; set; }
        [JsonProperty("fdv_to_tvl_ratio")]
        public decimal? FdvToTvlRatio { get; set; }
        public decimal? Roi { get; set; }
        public AthResponse? Ath { get; set; }
        [JsonProperty("ath_change_percentage")] 
        public AthChangePercentageResponse? AthChangePercentage { get; set; }
        [JsonProperty("ath_date")] 
        public AthDateResponse? AthDate { get; set; }
        public AtlResponse? Atl { get; set; }
        [JsonProperty("atl_change_percentage")] 
        public AtlChangePercentageResponse? AtlChangePercentage { get; set; }
        [JsonProperty("atl_date")] 
        public AtlDateResponse? AtlDate { get; set; }
        [JsonProperty("market_cap")] 
        public MarketCapResponse? MarketCap { get; set; }
        [JsonProperty("market_cap_rank")] 
        public decimal? MarketCapRank { get; set; }
        [JsonProperty("fully_diluted_valuation")] 
        public FullyDilutedValuationResponse? FullyDilutedValuation { get; set; }
        [JsonProperty("market_cap_fdv_ratio")] 
        public decimal? MarketCapFdvRatio { get; set; }
        [JsonProperty("total_volume")] 
        public TotalVolumeResponse? TotalVolume { get; set; }
        [JsonProperty("high_24h")] 
        public High24hResponse? High24h { get; set; }
        [JsonProperty("low_24h")] 
        public Low24hResponse? Low24h { get; set; }
        [JsonProperty("price_change_24h")] 
        public decimal? PriceChange24h { get; set; }
        [JsonProperty("price_change_percentage_24h")]
        public decimal? PriceChangePercentage24h { get; set; }
        [JsonProperty("price_change_percentage_7d")]
        public decimal? PriceChangePercentage7d { get; set; }
        [JsonProperty("price_change_percentage_14d")]
        public decimal? PriceChangePercentage14d { get; set; }
        [JsonProperty("price_change_percentage_30d")]
        public decimal? PriceChangePercentage30d { get; set; }
        [JsonProperty("price_change_percentage_60d")]
        public decimal? PriceChangePercentage60d { get; set; }
        [JsonProperty("price_change_percentage_200d")]
        public decimal? PriceChangePercentage200d { get; set; }
        [JsonProperty("price_change_percentage_1y")]
        public decimal? PriceChangePercentage1y { get; set; }
        [JsonProperty("market_cap_change_24h")]
        public decimal? MarketCapChange24h { get; set; }
        [JsonProperty("market_cap_change_percentage_24h")]
        public decimal? MarketCapChangePercentage24h { get; set; }
        [JsonProperty("price_change_24h_in_currency")]
        public PriceChange24hInCurrencyResponse? PriceChange24hInCurrency { get; set; }
        [JsonProperty("price_change_percentage_1h_in_currency")]
        public PriceChangePercentage1hInCurrencyResponse? PriceChangePercentage1hInCurrency { get; set; }
        [JsonProperty("price_change_percentage_24h_in_currency")]
        public PriceChangePercentage24hInCurrencyResponse? PriceChangePercentage24hInCurrency { get; set; }
        [JsonProperty("price_change_percentage_7d_in_currency")]
        public PriceChangePercentage7dInCurrencyResponse? PriceChangePercentage7dInCurrency { get; set; }
        [JsonProperty("price_change_percentage_14d_in_currency")]
        public PriceChangePercentage14dInCurrencyResponse? PriceChangePercentage14dInCurrency { get; set; }
        [JsonProperty("price_change_percentage_30d_in_currency")]
        public PriceChangePercentage30dInCurrencyResponse? PriceChangePercentage30dInCurrency { get; set; }
        [JsonProperty("price_change_percentage_60d_in_currency")]
        public PriceChangePercentage60dInCurrencyResponse? PriceChangePercentage60dInCurrency { get; set; }
        [JsonProperty("price_change_percentage_200d_in_currency")]
        public PriceChangePercentage200dInCurrencyResponse? PriceChangePercentage200dInCurrency { get; set; }
        [JsonProperty("price_change_percentage_1y_in_currency")]
        public PriceChangePercentage1yInCurrencyResponse? PriceChangePercentage1yInCurrency { get; set; }
        [JsonProperty("market_cap_change_24h_in_currency")]
        public MarketCapChange24hInCurrencyResponse? MarketCapChange24hInCurrency { get; set; }
        [JsonProperty("market_cap_change_percentage_24h_in_currency")]
        public MarketCapChangePercentage24hInCurrencyResponse? MarketCapChangePercentage24hInCurrency { get; set; }
        [JsonProperty("total_supply")]
        public decimal? TotalSupply { get; set; }
        [JsonProperty("max_supply")]
        public decimal? MaxSupply { get; set; }
        [JsonProperty("circulating_supply")]
        public decimal? CirculatingSupply { get; set; }
        [JsonProperty("last_updated")]
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
        [JsonProperty("web_slug")]
        public string? WebSlug { get; set; }
        [JsonProperty("asset_platform_id")]
        public string? AssetPlatformId { get; set; }
        public PlatformsResponse? Platforms { get; set; }
        [JsonProperty("detail_platforms")]
        public DetailPlatformsResponse? DetailPlatforms { get; set; }
        [JsonProperty("block_time_in_minutes")] 
        public long? BlockTimeInMinutes { get; set; }
        [JsonProperty("hashing_algorithm")]
        public object? HashingAlgorithm { get; set; }
        public List<string?>? Categories { get; set; }
        [JsonProperty("preview_listing")] 
        public bool? PreviewListing { get; set; }
        [JsonProperty("public_notice")] 
        public object? PublicNotice { get; set; }
        [JsonProperty("additional_notices")] 
        public List<object?>? AdditionalNotices { get; set; }
        
        public LocalizationResponse? Localization { get; set; }
        public DescriptionResponse? Description { get; set; }
        public LinksResponse? Links { get; set; }
        public ImageResponse? Image { get; set; }
        [JsonProperty("country_origin")] 
        public string? CountryOrigin { get; set; }
        [JsonProperty("genesis_date")]
        public object? GenesisDate { get; set; }
        [JsonProperty("contract_address")]
        public string? ContractAddress { get; set; }
        [JsonProperty("sentiment_votes_up_percentage")]
        public decimal? SentimentVotesUpPercentage { get; set; }
        [JsonProperty("sentiment_votes_down_percentage")]
        public decimal? SentimentVotesDownPercentage { get; set; }
        [JsonProperty("watchlist_portfolio_users")]
        public decimal? WatchlistPortfolioUsers { get; set; }
        [JsonProperty("market_cap_rank")]
        public decimal? MarketCapRank { get; set; }
        [JsonProperty("market_data")]
        public MarketDataResponse? MarketData { get; set; }
        [JsonProperty("community_data")] 
        public CommunityDataResponse? CommunityData { get; set; }
        [JsonProperty("developer_data")] 
        public DeveloperDataResponse? DeveloperData { get; set; }
        [JsonProperty("status_updates")]
        public List<object?>? StatusUpdates { get; set; }
        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }
        public List<TickerResponse>? Tickers { get; set; }
    }

    public class SolanaResponse
    {
        [JsonProperty("decimal_place")]
        public int? DecimalPlace { get; set; }
        [JsonProperty("contract_address")]
        public string? ContractAddress { get; set; }
    }

    public class TickerResponse
    {
        public string? Base { get; set; }
        public string? Target { get; set; }
        public MarketResponse? Market { get; set; }
        public decimal? Last { get; set; }
        public decimal? Volume { get; set; }
        [JsonProperty("converted_last")]
        public ConvertedLastResponse? ConvertedLast { get; set; }
        public ConvertedVolumeResponse? ConvertedVolume { get; set; }
        [JsonProperty("trust_score")]
        public string? TrustScore { get; set; }
        [JsonProperty("bid_ask_spread_percentage")]
        public decimal? BidAskSpreadPercentage { get; set; }
        public DateTime? Timestamp { get; set; }
        [JsonProperty("last_traded_at")]
        public DateTime? LastTradedAt { get; set; }
        [JsonProperty("last_fetch_at")]
        public DateTime? LastFetchAt { get; set; }
        [JsonProperty("is_anomaly")]
        public bool? IsAnomaly { get; set; }
        [JsonProperty("is_stale")]
        public bool? IsStale { get; set; }
        [JsonProperty("trade_url")]
        public string? TradeUrl { get; set; }
        [JsonProperty("token_info_url")]
        public object? TokenInfoUrl { get; set; }
        [JsonProperty("coin_id")]
        public string? CoinId { get; set; }
        [JsonProperty("target_coin_id")]
        public string? TargetCoinId { get; set; }
        
    }

    public class TotalVolumeResponse
    {
        public decimal? Usd { get; set; }
    }
}
