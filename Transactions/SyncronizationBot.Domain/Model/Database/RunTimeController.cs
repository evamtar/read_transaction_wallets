using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class RunTimeController : Entity
    {
        public int? IdRuntime { get; set; }
        public decimal? ConfigurationTimer { get; set; }
        public ETypeService TypeService { get; set; }
        public bool? IsRunning { get; set; }
        public bool? IsContingecyTransactions { get; set; }
        public int? TimesWithoutTransactions { get; set; }
    }
}
