

namespace SyncronizationBot.Domain.Model.CrossCutting.Prices.Response
{
    public class JupiterPricesResponse
    {
        public Dictionary<string, TokenData>? Data { get; set; }
        public decimal? TimeTaken { get; set; }
    }
    public class TokenData
    {
        public string? Id { get; set; }
        public string? MintSymbol { get; set; }
        public string? VsToken { get; set; }
        public string? VsTokenSymbol { get; set; }
        public decimal? Price { get; set; }
    }
}
