

using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Response
{
    public class ReadTokenPriceCommandResponse
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
        public decimal? UniqueWallet24H { get; set; }
        public decimal? UniqueWalletHistory24H { get; set; }
        public DateTime? CreateDate { get; set; }
        public EFontType FontPrice { get; set; }
        public bool IsSuccess { get; set; }
    }

    
}
