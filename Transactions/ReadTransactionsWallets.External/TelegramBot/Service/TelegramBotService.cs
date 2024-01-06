using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.TelegramBot.Configs;

namespace ReadTransactionsWallets.Infra.CrossCutting.TelegramBot.Service
{
    public class TelegramBotService: ITelegramBotService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TelegramBotConfig> _config;
        public TelegramBotService(HttpClient httpClient, IOptions<TelegramBotConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }

        public async Task<TelegramBotResponse> ExecuteRecoveryChatAsync(TelegramBotRequest request)
        {
            var response = await this._httpClient.PostAsync(string.Format(this._config.Value.ParametersUrlPostChannel ?? string.Empty, this._config.Value.Token ?? string.Empty), null);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TelegramBotResponse>(responseBody) ?? new TelegramBotResponse { };
        }

        public async Task<TelegramBotResponse> ExecuteSendMessageAsync(TelegramBotRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(this._config.Value.ParametersUrlSendMessage ?? string.Empty, this._config.Value.Token ?? string.Empty, request.ChatId, request.Message));
            var responseBody = await response.Content.ReadAsStringAsync();
            return new TelegramBotResponse { };
        }
    }
}
