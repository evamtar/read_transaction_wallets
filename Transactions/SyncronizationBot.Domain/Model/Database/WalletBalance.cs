using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalance : Entity
    {
        public Guid? WalletId { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Quantity { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Price { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? TotalValueUSD { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Wallet? Wallet { get; set; }
    }
}
