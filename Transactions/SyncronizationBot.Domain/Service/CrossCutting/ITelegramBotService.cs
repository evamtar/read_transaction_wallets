using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITelegramBotService
    {
        Task<TelegramBotResponse> ExecuteRecoveryChatAsync(TelegramBotRequest request);
        Task<TelegramBotResponse> ExecuteSendMessageAsync(TelegramBotRequest request);
    }
}
