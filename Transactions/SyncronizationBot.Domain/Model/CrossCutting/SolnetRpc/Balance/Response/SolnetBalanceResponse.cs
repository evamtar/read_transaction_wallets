

namespace SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response
{
    public class SolnetBalanceResponse
    {
        public bool? IsSuccess { get;set; }
        public DateTime? DateLoadBalance { get;set; }
        public List<BalanceResponse>? Result { get; set; }
    }

    public class BalanceResponse 
    { 
        public decimal? Amount { get; set; }
        public TokenResponse? Token { get; set; }
    }

    public class TokenResponse 
    { 
        public string? Hash { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public int? Decimals { get; set; }
    }
}
