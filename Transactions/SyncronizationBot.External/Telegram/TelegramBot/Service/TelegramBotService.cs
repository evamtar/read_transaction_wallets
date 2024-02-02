using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;
using SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Telegram.TelegramBot.Service
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TelegramBotConfig> _config;
        public TelegramBotService(HttpClient httpClient, IOptions<TelegramBotConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }

        public async Task<TelegramBotChannelUpdateResponse> ExecuteRecoveryChannelUpdatesAsync(TelegramBotChannelUpdateRequest request)
        {
            var response = await _httpClient.PostAsync(string.Format(_config.Value.ParametersUrlPostChannel ?? string.Empty, _config.Value.Token ?? string.Empty), null);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TelegramBotChannelUpdateResponse>(responseBody) ?? new TelegramBotChannelUpdateResponse { };
        }

        public async Task<TelegramBotMessageDeleteResponse> ExecuteDeleteMessagesAsync(TelegramBotMessageDeleteRequest request)
        {
            var response = await _httpClient.PostAsync(string.Format(_config.Value.ParametersUrlDeleteMessage?? string.Empty, _config.Value.Token ?? string.Empty, request.ChatId, request.MessageId), null);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TelegramBotMessageDeleteResponse>(responseBody) ?? new TelegramBotMessageDeleteResponse { };
        }

        public async Task<TelegramBotMessageSendResponse> ExecuteSendMessageAsync(TelegramBotMessageSendRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrlSendMessage ?? string.Empty, _config.Value.Token ?? string.Empty, request.ChatId, request.Message));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TelegramBotMessageSendResponse>(responseBody) ?? new TelegramBotMessageSendResponse { }; ;
        }
    }
}
