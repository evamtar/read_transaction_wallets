using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Telegram
{
    public interface ITelegramBotService
    {
        Task<TelegramBotChatResponse> ExecuteRecoveryChatAsync(TelegramBotChatRequest request);
        Task<TelegramBotMessageResponse> ExecuteSendMessageAsync(TelegramBotMessageRequest request);
    }
}
