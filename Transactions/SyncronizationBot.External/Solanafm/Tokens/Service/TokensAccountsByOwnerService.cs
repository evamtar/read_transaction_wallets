using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Configs;
using System.Net.Http;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Service
{
    public class TokensAccountsByOwnerService : ITokensAccountsByOwnerService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TokensAccountsByOwnerConfig> _config;
        public TokensAccountsByOwnerService(HttpClient httpClient, IOptions<TokensAccountsByOwnerConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            foreach (var header in this._config.Value.Headers!)
            {
                this._httpClient.DefaultRequestHeaders.Add(header.Key!, header.Value!);
            }
        }
        public Task<TokensResponse> ExecuteRecoveryTokensAccountsByOwnerAsync(TokensAccountsByOwnerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
