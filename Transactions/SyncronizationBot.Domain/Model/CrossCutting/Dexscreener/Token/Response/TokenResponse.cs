

namespace SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Response
{
    public class TokenResponse
    {
        public string ?SchemaVersion { get; set; }
        public List<Pair>? Pairs { get; set; }
    }
    public class Pair
    {
        public string? ChainId { get; set; }
        public string? DexId { get; set; }
        public string? Url { get; set; }
        public string? PairAddress { get; set; }
        public List<string>? Labels { get; set; }
        public TokenResult? BaseToken { get; set; }
        public TokenResult? QuoteToken { get; set; }
        public string? PriceNative { get; set; }
        public string? PriceUsd { get; set; }
        public Txns? Txns { get; set; }
        public VolumeResponse? Volume { get; set; }
        public PriceChangeResponse? PriceChange { get; set; }
        public LiquidityResponse? Liquidity { get; set; }
        public int Fdv { get; set; }
        public long PairCreatedAt { get; set; }
        public InfoResponse? Info { get; set; }
    }

    public class TokenResult 
    {
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Symbol { get; set; }
    }

    public class Txns
    {
        public TransactionResponse? M5 { get; set; }
        public TransactionResponse? H1 { get; set; }
        public TransactionResponse? H6 { get; set; }
        public TransactionResponse? H24 { get; set; }
    }

    public class TransactionResponse
    {
        public int Buys { get; set; }
        public int Sells { get; set; }
    }

    public class VolumeResponse
    {
        public double H24 { get; set; }
        public double H6 { get; set; }
        public double H1 { get; set; }
        public double M5 { get; set; }
    }

    public class PriceChangeResponse
    {
        public double M5 { get; set; }
        public double H1 { get; set; }
        public double H6 { get; set; }
        public double H24 { get; set; }
    }

    public class LiquidityResponse
    {
        public double Usd { get; set; }
        public double Base { get; set; }
        public double Quote { get; set; }
    }

    public class InfoResponse
    {
        public string? ImageUrl { get; set; }
        public List<WebsiteResponse>? Websites { get; set; }
        public List<SocialREsponse>? Socials { get; set; }
    }

    public class WebsiteResponse
    {
        public string? Label { get; set; }
        public string? Url { get; set; }
    }

    public class SocialREsponse
    {
        public string? Type { get; set; }
        public string? Url { get; set; }
    }
    
}
