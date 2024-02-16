using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TelegramChannel : Entity
    {
        public decimal? ChannelId { get; set; }
        public string? ChannelName { get; set; }
        public int? TimeBeforeDelete { get; set; }
        public virtual List<AlertPrice>? AlertPrices { get; set; }
        public virtual List<AlertConfiguration>? AlertsConfigurations { get; set; }
        public virtual List<TelegramMessage>? TelegramMessages { get; set; }
    }
}
