using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Telegram
{
    public interface ITelegramBotService
    {
        Task<TelegramBotChannelUpdateResponse> ExecuteRecoveryChannelUpdatesAsync(TelegramBotChannelUpdateRequest request);
        Task<TelegramBotMessageDeleteResponse> ExecuteDeleteMessagesAsync(TelegramBotMessageDeleteRequest request);
        Task<TelegramBotMessageSendResponse> ExecuteSendMessageAsync(TelegramBotMessageSendRequest request);
    }
}
