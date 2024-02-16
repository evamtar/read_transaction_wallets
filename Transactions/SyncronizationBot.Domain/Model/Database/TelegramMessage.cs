using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TelegramMessage : Entity
    {
        public long MessageId { get; set; }
        public Guid? TelegramChannelId { get; set; }
        public Guid? EntityId { get; set; }
        public DateTime? DateSended { get; set; }
        public bool? IsDeleted { get; set; }
        public int? TryDeleted { get; set; }
        public TelegramChannel? TelegramChannel { get; set; }
    }
}
