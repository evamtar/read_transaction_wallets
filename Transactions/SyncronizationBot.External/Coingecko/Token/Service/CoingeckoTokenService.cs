using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Coingecko.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Coingecko.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Coingecko;
using SyncronizationBot.Infra.CrossCutting.Coingecko.Token.Configs;
using System.Net;



namespace SyncronizationBot.Infra.CrossCutting.Coingecko.Token.Service
{
    public class CoingeckoTokenService : ICoingeckoTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<CoingeckoTokenConfig> _config;
        public CoingeckoTokenService(HttpClient httpClient, IOptions<CoingeckoTokenConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TokenResponse> ExecuteRecoveryTokenAsync(TokenRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, _config.Value.Chain ?? string.Empty, request.TokenHash ?? string.Empty));
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null!;
                throw new Exception(responseBody);
            }
            return JsonConvert.DeserializeObject<TokenResponse>(responseBody) ?? new TokenResponse { };
        }
    }
}
