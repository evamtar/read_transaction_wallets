

using System.Text.Json.Serialization;

namespace ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Response
{
    public class TelegramBotResponse
    {
        public bool? Ok { get; set; }
        public List<ResultResponse>? Result { get; set; }
    }
    public class ResultResponse
    {
        [JsonPropertyName("update_id")]
        public long? UpdateId { get; set; }
        [JsonPropertyName("my_chat_member")]
        public ChatMemberResponse? ChatMember {get;set;}
    }
    public class ChatMemberResponse 
    { 
        public ChatResponse? Chat { get; set; }
    }
    public class ChatResponse
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }
}
