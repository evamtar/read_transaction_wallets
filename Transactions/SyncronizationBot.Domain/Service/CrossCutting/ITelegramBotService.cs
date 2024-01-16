using SyncronizationBot.Domain.Model.CrossCutting.TelegramBot.Request;
using SyncronizationBot.Domain.Model.CrossCutting.TelegramBot.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITelegramBotService
    {
        Task<TelegramBotResponse> ExecuteRecoveryChatAsync(TelegramBotRequest request);
        Task<TelegramBotResponse> ExecuteSendMessageAsync(TelegramBotRequest request);
    }
}
