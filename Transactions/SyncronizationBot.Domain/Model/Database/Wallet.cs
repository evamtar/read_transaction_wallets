using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class Wallet : Entity
    {
        public string? Hash { get; set; }
        public Guid? ClassWalletId { get; set; }
        public bool? IsLoadBalance { get; set; }
        public DateTime? DateLoadBalance { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }
        public virtual ClassWallet? ClassWallet { get; set; }
        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<TransactionRPCRecovery>? TransactionsRPCRecovery { get; set; }
        public virtual List<WalletBalance>? Balances { get; set; }
        public virtual List<TokenAlphaWallet>? TokenAlphas { get; set; }
    }
}
