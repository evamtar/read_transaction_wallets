

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request
{
    public class TelegramBotMessageRequest
    {
        public long? ChatId { get; set; }
        public string? Message { get; set; }
    }
}
