using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Token : Entity
    {
        public string? Hash { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public decimal? Supply { get; set; }
        public int? Decimals { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool? IsLazyLoad {get;set;}
        public virtual List<WalletBalance>? Balances { get; set; }
        public virtual List<TokenSecurity>? TokenSecurities { get; set; }
        public virtual List<TokenAlpha>? TokenAlphas { get; set; }
        public virtual List<TransactionToken>? TransactionTokens { get; set; }
        public virtual List<TokenPriceHistory>? TokenPriceHistories { get; set; }
    }
}
