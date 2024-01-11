using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class Token : Entity
    {
        public string? Hash { get; set; }
        public string? TokenAlias { get; set; }
        public string? Symbol { get; set; }
        public string? TokenType { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }
        public int? Decimals { get; set; }

        public virtual List<Transactions>? TransactionsSource { get; set; }
        public virtual List<Transactions>? TransactionsSourcePool { get; set; }
        public virtual List<Transactions>? TransactionsDestination { get; set; }
        public virtual List<Transactions>? TransactionsDestinationPool { get; set; }
        public virtual List<WalletBalance>? Balances { get; set; }
    }
}
