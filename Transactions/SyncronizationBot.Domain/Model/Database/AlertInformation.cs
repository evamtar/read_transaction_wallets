using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertInformation : Entity
    {
        public string? Message { get; set; }
        public int? IdSubLevel { get; set; }
        public Guid? AlertConfigurationId { get; set; }
        public virtual AlertConfiguration? AlertConfiguration { get; set;}
        public virtual List<AlertParameter>? AlertsParameters { get; set; }
    }
}

