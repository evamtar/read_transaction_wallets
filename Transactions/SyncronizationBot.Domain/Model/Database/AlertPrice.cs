using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertPrice : Entity
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PriceBase { get; set; }
        public string? TokenHash { get; set; }
        public decimal? PriceValue { get; set; }
        public decimal? PricePercent { get; set; }
        public ETypeAlertPrice TypeAlert { get; set; }
        public bool? IsRecurrence { get; set; }
        public Guid? TelegramChannelId { get; set; }
        public virtual TelegramChannel? TelegramChannel { get; set; }
    }
}
