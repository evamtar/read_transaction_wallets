using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertConfiguration : Entity
    {
        public string? Name { get; set; }
        public Guid? TypeOperationId { get; set; }
        public Guid? TelegramChannelId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual TelegramChannel? TelegramChannel { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual List<AlertInformation>? AlertsInformations { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual TypeOperation? TypeOperation { get; set; }
    }
}
