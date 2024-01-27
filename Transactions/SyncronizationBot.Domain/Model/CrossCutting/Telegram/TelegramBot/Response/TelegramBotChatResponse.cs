using Newtonsoft.Json;

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response
{
    public class TelegramBotChatResponse
    {
        public bool? Ok { get; set; }
        public List<TelegramBotChatResultResponse>? Result { get; set; }
    }
    public class TelegramBotChatResultResponse
    {
        [JsonProperty("update_id")]
        public long? UpdateId { get; set; }
        [JsonProperty("my_chat_member")]
        public TelegramBotChatChatMemberResponse? ChatMember { get; set; }
    }
    public class TelegramBotChatChatMemberResponse
    {
        public TelegramBotChatChatResponse? Chat { get; set; }
    }
    public class TelegramBotChatChatResponse
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }
}
