

namespace SyncronizationBot.Application.Commands.Base
{
    public class SearchCommand
    {
        public DateTime? DateLoadBalance { get; set; }
        public decimal InitialTicks { get; set; }
        public decimal FinalTicks { get; set; }
    }
}
