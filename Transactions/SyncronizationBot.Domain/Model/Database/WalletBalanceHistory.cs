using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalanceHistory : Entity
    {
        public Guid? WalletBalanceId { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? OldQuantity { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? NewQuantity { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? RequestQuantity { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PercentageCalculated { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Price { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? TotalValueUSD { get; set; }
        public string? Signature { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
