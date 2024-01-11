using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class TransactionNotMapped : Entity
    {
        public string? Signature { get; set; }
        public string? Link { get; set; }
        public string? Error { get; set; }
        public string? StackTrace { get; set; }
        public DateTime? DateTimeRunner { get; set; }
    }
}
