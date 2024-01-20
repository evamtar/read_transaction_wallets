using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class ClassWallet : Entity
    {
        public int? IdClassification { get; set; }
        public string? Description { get; set; }
        public virtual List<Wallet>? Wallets { get; set; }
    }
}
