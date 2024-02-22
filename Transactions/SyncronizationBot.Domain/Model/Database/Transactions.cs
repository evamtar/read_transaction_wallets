using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateTransactionUTC { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? FeeTransaction { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceSol { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? TotalOperationSol { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? TypeOperationId { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Wallet? Wallet { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual TypeOperation? TypeOperation { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        [DbSqlServerMapper(SqlServerTarget.Ignore)]
        public virtual string? WalletHash { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        [DbSqlServerMapper(SqlServerTarget.Ignore)]
        public virtual string? ClassWallet { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<TransactionToken>? TransactionTokens { get; set; }
    }
}
