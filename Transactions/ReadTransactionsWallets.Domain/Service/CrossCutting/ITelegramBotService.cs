using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Response;


namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface ITelegramBotService
    {
        Task<TelegramBotResponse> ExecuteRecoveryChatAsync(TelegramBotRequest request);
        Task<TelegramBotResponse> ExecuteSendMessageAsync(TelegramBotRequest request);
    }
}
