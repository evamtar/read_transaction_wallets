

namespace SyncronizationBot.Application.Response
{
    public class RecoveryAddUpdateBalanceItemCommandResponse
    {
        public decimal? PercentModify { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public bool? IsActive { get; set; }
    }
}
