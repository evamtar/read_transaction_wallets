

namespace SyncronizationBot.Application.Commands.Base
{
    public class TransactionsCommand
    {
        public string? WalletHash { get; set; }
        public DateTime? DateLoadBalance { get; set; }
        public decimal InitialTicks { get; set; }
        public decimal FinalTicks { get; set; }
    }
}
