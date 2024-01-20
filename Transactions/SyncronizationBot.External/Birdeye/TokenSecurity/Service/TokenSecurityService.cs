using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenSecurity.Configs;



namespace SyncronizationBot.Infra.CrossCutting.Birdeye.TokenSecurity.Service
{
    public class TokenSecurityService : ITokenSecurityService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokenSecurityConfig> _config;
        public TokenSecurityService(HttpClient httpClient, IOptions<TokenSecurityConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("X-API-KEY", config.Value.ApiKey ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("x-chain", config.Value.Chain ?? string.Empty);
        }
        public async Task<TokenSecurityResponse> ExecuteRecoveryTokenCreationAsync(TokenSecurityRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.TokenHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenSecurityResponse>(responseBody) ?? new TokenSecurityResponse { };
        }
    }
}
