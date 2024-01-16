using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response;
using SyncronizationBot.Domain.Service.CrossCutting;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Service
{
    public class TokensService : ITokensService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokensConfig> _config;
        public TokensService(HttpClient httpClient, IOptions<TokensConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _config.Value.ApiKey ?? string.Empty);
        }

        public async Task<TokensResponse> ExecuteRecoveryTokensAsync(TokensRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.TokenHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokensResponse>(responseBody) ?? new TokensResponse { };
        }
    }
}
