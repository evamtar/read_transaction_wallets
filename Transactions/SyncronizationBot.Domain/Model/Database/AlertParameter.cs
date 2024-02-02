using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertParameter : Entity
    {
        public string? Name { get; set; }
        public Guid? AlertInformationId { get; set; }
        public string? Class { get; set; }
        public string? Parameter { get; set; }
        public string? FixValue { get; set; }
        public string? DefaultValue { get; set; }
        public bool? HasAdjustment { get; set; }
        public bool? IsIcon { get; set; }
        public bool? IsImage { get; set; }

        public virtual AlertInformation? AlertInformation { get; set; }

    }
}
