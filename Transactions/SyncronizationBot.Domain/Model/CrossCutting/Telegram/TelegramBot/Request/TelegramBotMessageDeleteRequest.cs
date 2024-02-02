

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request
{
    public class TelegramBotMessageDeleteRequest
    {
        public long? ChatId { get; set; }
        public long? MessageId { get; set; }
    }
}
