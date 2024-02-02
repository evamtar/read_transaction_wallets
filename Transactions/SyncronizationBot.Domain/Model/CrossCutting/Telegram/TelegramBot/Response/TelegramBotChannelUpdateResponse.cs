using Newtonsoft.Json;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response
{
    public class TelegramBotChannelUpdateResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("result")]
        public List<TelegramBotChannelUpdateResult>? Result { get; set; }
    }

    public class TelegramBotChannelUpdateResult
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }

        [JsonProperty("my_chat_member")]
        public TelegramBotChannelUpdateChatMember? MyChatMember { get; set; }

        [JsonProperty("channel_post")]
        public TelegramBotChannelUpdateChannelPost? ChannelPost { get; set; }
    }

    public class TelegramBotChannelUpdateChatMember
    {
        [JsonProperty("chat")]
        public TelegramBotChannelUpdateChat? Chat { get; set; }

        [JsonProperty("from")]
        public TelegramBotChannelUpdateUser? From { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("old_chat_member")]
        public TelegramBotChannelUpdateChatMemberDetails? OldChatMember { get; set; }

        [JsonProperty("new_chat_member")]
        public TelegramBotChannelUpdateChatMemberDetails? NewChatMember { get; set; }
    }

    public class TelegramBotChannelUpdateChatMemberDetails
    {
        [JsonProperty("user")]
        public TelegramBotChannelUpdateUser? User { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("until_date")]
        public long UntilDate { get; set; }
    }

    public class TelegramBotChannelUpdateChat
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }

    public class TelegramBotChannelUpdateUser
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("language_code")]
        public string? LanguageCode { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }
    }

    public class TelegramBotChannelUpdateChannelPost
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }

        [JsonProperty("sender_chat")]
        public TelegramBotChannelUpdateChat? SenderChat { get; set; }

        [JsonProperty("chat")]
        public TelegramBotChannelUpdateChat? Chat { get; set; }

        [JsonProperty("date")]
        public long? Date { get; set; }
        public DateTime? DateOfMessage { 
            get 
            {
                if(this.Date.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.Date.Value);
                return null;
            } 
        }

        [JsonProperty("text")]
        public string? Text { get; set; }
    }
}
