using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateTransactionUTC { get; set; }
        public decimal? FeeTransaction { get; set; }
        public decimal? PriceSol { get; set; }
        public decimal? TotalOperationSol { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? TypeOperationId { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual TypeOperation? TypeOperation { get; set; }
        public virtual string? WalletHash { get; set; }
        public virtual string? ClassWallet { get; set; }
        public List<TransactionToken>? TransactionTokens { get; set; }
    }
}
