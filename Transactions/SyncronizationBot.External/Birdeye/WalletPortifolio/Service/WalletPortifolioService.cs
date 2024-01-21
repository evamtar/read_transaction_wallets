using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Infra.CrossCutting.Birdeye.WalletPortofolio.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Birdeye.WalletPortifolio.Service
{
    public class WalletPortifolioService : IWalletPortifolioService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<WalletPortifolioConfig> _config;
        public WalletPortifolioService(HttpClient httpClient, IOptions<WalletPortifolioConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("X-API-KEY", config.Value.ApiKey ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("x-chain", config.Value.Chain ?? string.Empty);
            this._httpClient.Timeout = TimeSpan.FromMinutes(10);
        }
        public async Task<WalletPortifolioResponse> ExecuteRecoveryWalletPortifolioAsync(WalletPortifolioRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.WalletHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WalletPortifolioResponse>(responseBody) ?? new WalletPortifolioResponse { };
        }
    }
}
