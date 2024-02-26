

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Response
{
    public class ReadTokenFullInfoCommandResponse
    {
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public decimal? NumberOfMarkets { get; set; }
        public decimal? PriceUsd { get; set; }
        public decimal? PriceChange5m { get; set; }
        public decimal? PriceChange30m { get; set; }
        public decimal? PriceChange1h { get; set; }
        public decimal? PriceChange4h { get; set; }
        public decimal? PriceChange6h { get; set; }
        public decimal? PriceChange24 { get; set; }
        public decimal? Liquidity { get; set; }
        public decimal? Marketcap { get; set; }
        public decimal? Supply { get; set; }
        public decimal? Decimals { get; set; }
        public string? MintAuthority { get; set; }
        public string? FreezeAuthority { get; set; }
        public decimal? UniqueWallet24H { get; set; }
        public decimal? UniqueWalletHistory24H { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsSuccess { get; set; }
    }
}
