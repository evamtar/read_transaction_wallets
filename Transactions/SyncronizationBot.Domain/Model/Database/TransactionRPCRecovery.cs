using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TransactionRPCRecovery : Entity
    {
        public string? Signature { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? BlockTime { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public Guid? WalletId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsIntegrated { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Wallet? Wallet { get; set; }
    }
}
