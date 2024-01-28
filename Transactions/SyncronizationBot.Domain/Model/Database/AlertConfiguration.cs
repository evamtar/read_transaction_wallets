using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertConfiguration : Entity
    {
        public string? Name { get; set; }
        public ETypeAlert? TypeAlert { get; set; }
        public Guid? TelegramChannelId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual TelegramChannel? TelegramChannel { get; set; }
        public virtual List<AlertInformation>? AlertsInformations { get; set; }
    }
}
