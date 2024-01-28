using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TransactionNotMapped : Entity
    {
        public Guid? IdWallet { get; set; }
        public string? Signature { get; set; }
        public string? Link { get; set; }
        public string? Error { get; set; }
        public string? StackTrace { get; set; }
        public DateTime? DateTimeRunner { get; set; }
    }
}
