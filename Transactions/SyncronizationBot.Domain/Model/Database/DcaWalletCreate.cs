

using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class DcaWalletCreate : Entity
    {
        public Guid? WalletDCAId { get; set; }
        public string? WalletHash { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountExecuted { get; set; }
        public int? ExecutionTimeApproximated { get; set; }
        public ETypeTimeExecution? TypeTimeExecution { get; set; }
        public ETypeOperation TypeOperation { get; set; }
    }
}
