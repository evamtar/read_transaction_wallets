using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Token : Entity
    {
        public string? Hash { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public decimal? Supply { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Liquidity { get; set; }
        public int? UniqueWallet24h { get; set; }
        public int? UniqueWalletHistory24h { get; set; }
        public int? Decimals { get; set; }
        public int? NumberMarkets { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual List<Transactions>? TransactionsSource { get; set; }
        public virtual List<Transactions>? TransactionsSourcePool { get; set; }
        public virtual List<Transactions>? TransactionsDestination { get; set; }
        public virtual List<Transactions>? TransactionsDestinationPool { get; set; }
        public virtual List<WalletBalance>? Balances { get; set; }
        public virtual List<TokenSecurity>? TokenSecurities { get; set; }
    }
}
