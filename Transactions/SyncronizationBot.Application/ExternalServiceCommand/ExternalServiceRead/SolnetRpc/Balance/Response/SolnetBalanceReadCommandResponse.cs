
namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.SolnetRpc.Balance.Response
{
    public class SolnetBalanceReadCommandResponse
    {
        public DateTime? DateLoadBalance { get; set; }
        public bool? IsSuccess { get; set; }
        public List<SolnetReadBalanceResponse>? Result { get; set; }
    }
    public class SolnetReadBalanceResponse
    {
        public decimal? Amount { get; set; }
        public SolnetReadTokenResponse? Token { get; set; }
    }

    public class SolnetReadTokenResponse
    {
        public string? Hash { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public int? Decimals { get; set; }
    }
}
