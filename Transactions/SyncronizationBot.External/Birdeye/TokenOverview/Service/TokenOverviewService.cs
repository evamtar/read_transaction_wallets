using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Infra.CrossCutting.Birdeye.TokenOverview.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Birdeye.TokenOverview.Service
{
    public class TokenOverviewService : ITokenOverviewService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokenOverviewConfig> _config;
        public TokenOverviewService(HttpClient httpClient, IOptions<TokenOverviewConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("X-API-KEY", config.Value.ApiKey ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("x-chain", config.Value.Chain ?? string.Empty);
        }
        public async Task<TokenOverviewResponse> ExecuteRecoveryTokenOverviewAsync(TokenOverviewRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.TokenHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenOverviewResponse>(responseBody) ?? new TokenOverviewResponse { };
        }
    }
}
