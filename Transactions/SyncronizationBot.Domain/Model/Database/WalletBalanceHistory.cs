using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalanceHistory : Entity
    {
        public Guid? WalletBalanceId { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }
        public decimal? OldQuantity { get; set; }
        public decimal? NewQuantity { get; set; }
        public decimal? RequestQuantity { get; set; }
        public decimal? PercentageCalculated { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalValueUSD { get; set; }
        public string? Signature { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
