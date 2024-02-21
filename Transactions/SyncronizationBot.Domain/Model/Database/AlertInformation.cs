using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertInformation : Entity
    {
        public string? Message { get; set; }
        public int? IdSubLevel { get; set; }
        public Guid? AlertConfigurationId { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual AlertConfiguration? AlertConfiguration { get; set;}

        [DbMapper(MongoTarget.Ignore)]
        public virtual List<AlertParameter>? AlertsParameters { get; set; }
    }
}

