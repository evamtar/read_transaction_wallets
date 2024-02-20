using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class RunTimeController : Entity
    {
        public int? RuntimeId { get; set; }
        public decimal? ConfigurationTimer { get; set; }
        public ETypeService TypeService { get; set; }
        public bool? IsRunning { get; set; }
        public string? JobName { get; set; }
        public string? JobDescription { get; set; }
        public bool? IsActive { get; set; }
    }
}
