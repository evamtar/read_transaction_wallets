using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;
using SyncronizationBot.Infra.CrossCutting.Jupiter.Prices.Configs;

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
        public async Task<JupiterPricesResponse> ExecuteRecoveryPriceAsync(JupiterPricesRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, string.Join(",", request.Ids?? new List<string>())));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JupiterPricesResponse>(responseBody) ?? new JupiterPricesResponse { };
        }
    }
}
