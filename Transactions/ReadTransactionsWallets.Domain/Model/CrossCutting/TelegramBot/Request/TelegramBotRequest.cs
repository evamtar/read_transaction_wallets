
namespace ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Request
{
    public class TelegramBotRequest
    {
        public long? ChatId { get; set; }
        public string? Message { get; set; }
    }
}
