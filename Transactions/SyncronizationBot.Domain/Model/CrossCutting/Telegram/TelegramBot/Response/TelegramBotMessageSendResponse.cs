using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response
{
    public class TelegramBotMessageSendResponse
    {
        public bool Ok { get; set; }
        public TelegramBotMessageResultResponse? Result { get; set; }
    }

    public class TelegramBotMessageResultResponse
    {
        public int MessageId { get; set; }
        public string? AuthorSignature { get; set; }
        public TelegramBotMessageChatResponse? SenderChat { get; set; }
        public TelegramBotMessageChatResponse? Chat { get; set; }
        public int Date { get; set; }
        public string? Text { get; set; }
        public List<TelegramBotMessageEntityResponse>? Entities { get; set; }
    }

    public class TelegramBotMessageChatResponse
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
    }

    public class TelegramBotMessageEntityResponse
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public string? Type { get; set; }
    }
}
