

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request
{
    public class TelegramBotMessageSendRequest
    {
        public long? ChatId { get; set; }
        public string? Message { get; set; }
    }
}
