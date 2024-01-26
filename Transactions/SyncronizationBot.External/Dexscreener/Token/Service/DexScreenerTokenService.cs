using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Dexscreener;
using SyncronizationBot.Infra.CrossCutting.Dexscreener.Token.Configs;


namespace SyncronizationBot.Infra.CrossCutting.Dexscreener.Token.Service
{
    public class DexScreenerTokenService : IDexScreenerTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<DexScreenerTokenConfig> _config;
        public DexScreenerTokenService(HttpClient httpClient, IOptions<DexScreenerTokenConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }
        public async Task<TokenResponse> ExecuteRecoveryTokenAsync(TokenRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.TokenHash ?? string.Empty));
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return JsonConvert.DeserializeObject<TokenResponse>(responseBody) ?? new TokenResponse { };
        }
    }
}
