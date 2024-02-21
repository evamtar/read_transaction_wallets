using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalance : Entity
    {
        public Guid? WalletId { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Quantity { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Price { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? TotalValueUSD { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual Wallet? Wallet { get; set; }
    }
}
