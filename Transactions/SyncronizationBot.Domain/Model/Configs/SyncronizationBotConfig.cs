

using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Configs
{
    public class SyncronizationBotConfig
    {
        public double? GTMHoursAdjust { get; set; }
        public double? UTCTransactionMinutesAdjust { get; set;}
        public int? MaxTimesWithoutTransactions { get; set; }
        public bool InValidation { get; set; }
    }
}
