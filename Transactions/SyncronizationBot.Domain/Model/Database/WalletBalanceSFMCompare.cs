using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalanceSFMCompare : Entity
    {
        public Guid? IdWallet { get; set; }
        public Guid? IdToken { get; set; }
        public string? TokenHash { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalValueUSD { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }
        public virtual Token? Token { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
