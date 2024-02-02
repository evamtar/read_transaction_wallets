using Newtonsoft.Json;
using SyncronizationBot.Utils;
using System.Text.Json;


namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response
{
    public class TelegramBotMessageSendResponse
    {
        public bool Ok { get; set; }
        public TelegramBotMessageSendResultResponse? Result { get; set; }
    }

    public class TelegramBotMessageSendResultResponse
    {
        [JsonProperty("message_id")]
        public long? MessageId { get; set; }
        public string? AuthorSignature { get; set; }
        public TelegramBotMessageSendChatResponse? SenderChat { get; set; }
        public TelegramBotMessageSendChatResponse? Chat { get; set; }
        public long? Date { get; set; }
        public DateTime? DateOfMessage
        {
            get
            {
                if (this.Date.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.Date.Value);
                return null;
            }
        }
        public string? Text { get; set; }
        public List<TelegramBotMessageSendEntityResponse>? Entities { get; set; }
    }

    public class TelegramBotMessageSendChatResponse
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }

    public class TelegramBotMessageSendEntityResponse
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public string? Type { get; set; }
    }
}
