using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.CrossCutting.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Prices.Response;
using SyncronizationBot.Domain.Service.CrossCutting;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Config;

namespace SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Service
{
    public class JupiterPriceService : IJupiterPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<JupiterPriceConfig> _config;
        public JupiterPriceService(HttpClient httpClient, IOptions<JupiterPriceConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }
        public Task<JupiterPricesResponse> ExecuteRecoveryPriceAsync(JupiterPricesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
