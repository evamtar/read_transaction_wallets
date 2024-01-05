using ReadTransactionsWallets.Domain.Model.Database.Base;
using ReadTransactionsWallets.Domain.Model.Enum;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? AmountValue { get; set;}
        public Guid? IdToken { get; set; }
        public Guid? IdWallet { get; set; }
        public ETypeOperation TypeOperation { get; set; }
        public string? JsonResponse { get; set; }

        public virtual Token? Token { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
