using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertPrice : Entity
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? PriceBase { get; set; }
        public string? TokenHash { get; set; }
        public long? PriceValue { get; set; }
        public decimal? PricePercent { get; set; }
        public Guid? TelegramChannelId { get; set; }
        public virtual TelegramChannel? TelegramChannel { get; set; }
    }
}
