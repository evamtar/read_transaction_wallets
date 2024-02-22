using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TransactionToken : Entity
    {
        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? AmountValue { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? MtkcapToken { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? TotalToken { get; set; }
        public ETypeTokenTransaction? TypeTokenTransactionId { get; set; }
        public bool? IsArbitrationOperation { get; set; }
        public bool? IsPoolOperation { get; set; }
        public bool? IsSwapOperation { get; set; }
        public Guid? TokenId { get; set; }
        public Guid? TransactionId { get; set;}

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Transactions? Transactions { get; set; }
    }
}

