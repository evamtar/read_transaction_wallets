using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Tokens.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Tokens.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Tokens.Configs;

namespace ReadTransactionsWallets.Infra.CrossCutting.Tokens.Service
{
    public class TokensService : ITokensService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokensConfig> _config;
        public TokensService(HttpClient httpClient, IOptions<TokensConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", this._config.Value.ApiKey ?? string.Empty);
        }
        
        public async Task<TokensResponse> ExecuteRecoveryTokensAsync(TokensRequest request)
        {
            var response = await this._httpClient.GetAsync(string.Format(this._config.Value.ParametersUrl ?? string.Empty, request.TokenHash));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokensResponse>(responseBody) ?? new TokensResponse { };
        }
    }
}
