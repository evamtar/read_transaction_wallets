using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenCreation.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenCreation.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenCreation.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Birdeye.TokenCreation.Service
{
    public class TokenCreationService : ITokenCreationService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokenCreationConfig> _config;
        public TokenCreationService(HttpClient httpClient, IOptions<TokenCreationConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("X-API-KEY", config.Value.ApiKey ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("x-chain", config.Value.Chain ?? string.Empty);
        }

        public async Task<TokenCreationResponse> ExecuteRecoveryTokenCreationAsync(TokenCreationRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.TokenHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenCreationResponse>(responseBody) ?? new TokenCreationResponse { };
        }
    }
}
