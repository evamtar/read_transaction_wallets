
namespace SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Response
{
    public class WalletPortifolioResponse
    {
        public ResultData? Data { get; set; }
        public bool? Success { get; set; }
    }
    public class ResultData
    {
        public string? Wallet { get; set; }
        public decimal? TotalUsd { get; set; }
        public List<ResultItem>? Items { get; set; }
    }
    public class ResultItem
    {
        public string? Address { get; set; }
        public int? Decimals { get; set; }
        public decimal? Balance { get; set; }
        public decimal? UiAmount { get; set; }
        public string? ChainId { get; set; }
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public string? Icon { get; set; }
        public string? LogoURI { get; set; }
        public decimal? PriceUsd { get; set; }
        public decimal? ValueUsd { get; set; }
    }
}
