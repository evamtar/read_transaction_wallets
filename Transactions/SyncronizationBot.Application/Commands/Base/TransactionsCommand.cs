

namespace SyncronizationBot.Application.Commands.Base
{
    public class TransactionsCommand
    {
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public DateTime? DateLoadBalance { get; set; }
        public decimal InitialTicks { get; set; }
        public decimal FinalTicks { get; set; }
    }
}
