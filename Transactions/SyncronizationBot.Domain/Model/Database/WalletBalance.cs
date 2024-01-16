using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class WalletBalance : Entity
    {
        public DateTime? DateUpdate { get; set; }
        public Guid? IdWallet { get; set; }
        public Guid? IdToken { get; set; }
        public decimal? Quantity { get; set; }
        public virtual Token? Token { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
