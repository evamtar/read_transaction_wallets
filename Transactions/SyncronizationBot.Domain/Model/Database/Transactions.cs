using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class Transactions: Entity
    {
        public string? Signature { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public decimal? AmountValueSource { get; set; }
        public decimal? AmountValueSourcePool { get; set; }
        public decimal? AmountValueDestination { get; set;}
        public decimal? AmountValueDestinationPool { get; set; }
        public decimal? MtkcapTokenSource { get; set; }
        public decimal? MtkcapTokenSourcePool { get; set; }
        public decimal? MtkcapTokenDestination { get; set; }
        public decimal? MtkcapTokenDestinationPool { get; set; }
        public decimal? PriceSol { get; set; }
        public Guid? IdTokenSource { get; set; }
        public Guid? IdTokenSourcePool { get; set; }
        public Guid? IdTokenDestination { get; set; }
        public Guid? IdTokenDestinationPool { get; set; }
        public Guid? IdWallet { get; set; }
        public ETypeOperation TypeOperation { get; set; }
        public virtual Token? TokenSource { get; set; }
        public virtual Token? TokenSourcePool { get; set; }
        public virtual Token? TokenDestination { get; set; }
        public virtual Token? TokenDestinationPool { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
