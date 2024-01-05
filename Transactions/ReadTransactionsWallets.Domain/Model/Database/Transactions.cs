using ReadTransactionsWallets.Domain.Model.Database.Base;
using ReadTransactionsWallets.Domain.Model.Enum;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? AmountValueSource { get; set; }
        public decimal? AmountValueDestination { get; set;}
        public Guid? IdTokenSource { get; set; }
        public Guid? IdTokenDestination { get; set; }
        public Guid? IdWallet { get; set; }
        public ETypeOperation TypeOperation { get; set; }
        public string? JsonResponse { get; set; }

        public virtual Token? TokenSource { get; set; }
        public virtual Token? TokenDestination { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
