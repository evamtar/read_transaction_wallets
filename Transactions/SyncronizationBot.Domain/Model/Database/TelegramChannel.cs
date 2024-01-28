using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TelegramChannel : Entity
    {
        public decimal? ChannelId { get; set; }
        public string? ChannelName { get; set; }
        public virtual List<AlertPrice>? AlertPrices { get; set; }
        public virtual List<AlertConfiguration>? AlertsConfigurations { get; set; }
    }
}
