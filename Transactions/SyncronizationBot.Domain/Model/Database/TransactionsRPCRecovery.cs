using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TransactionsRPCRecovery : Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? BlockTime { get; set; }
        public Guid? WalletId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsIntegrated { get; set; }
        public bool IsDCA { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
