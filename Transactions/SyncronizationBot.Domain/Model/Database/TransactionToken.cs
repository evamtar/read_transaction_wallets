using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TransactionToken : Entity
    {
        public decimal? AmountValue { get; set; }
        public decimal? MtkcapToken { get; set; }
        public decimal? TotalToken { get; set; }
        public ETypeTokenTransaction? TypeTokenTransaction { get; set; }
        public bool? IsArbitrationOperation { get; set; }
        public bool? IsPoolOperation { get; set; }
        public bool? IsSwapOperation { get; set; }
        public Guid? TokenId { get; set; }
        public Guid? TransactionId { get; set;}
        public virtual Token? Token { get; set; }
        public virtual Transactions? Transactions { get; set; }
    }
}

