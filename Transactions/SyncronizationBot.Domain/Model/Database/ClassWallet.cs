using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class ClassWallet : Entity
    {
        public int? IdClassification { get; set; }
        public string? Description { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual List<Wallet>? Wallets { get; set; }
    }
}
