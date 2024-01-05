using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class ClassWallet : Entity
    {
        public int? IdClassification { get; set; }
        public string? Description { get; set; }
        public virtual List<Wallet>? Wallets { get; set; }
    }
}
