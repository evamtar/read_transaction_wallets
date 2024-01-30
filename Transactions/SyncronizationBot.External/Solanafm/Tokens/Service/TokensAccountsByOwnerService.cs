using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Configs;
using System.Text;

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
        public async Task<List<TokensAccountsByOwnerResponse>> ExecuteRecoveryTokensAccountsByOwnerAsync(TokensAccountsByOwnerRequest request)
        {
            var listReturn = new List<TokensAccountsByOwnerResponse>();
            if (this._config.Value.TokenProgramIds != null) 
            {
                foreach (var tokenProgramId in this._config.Value.TokenProgramIds)
                {
                    var data = this._config.Value.Data ?? string.Empty;
                    data = data.Replace("{{WalletPublicKeyHash}}", request.WalletPublicKeyHash);
                    data = data.Replace("{{TokenProgramId}}", tokenProgramId);
                    data = data.Replace("{{Id}}", request.ID?.ToString());
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("", content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        throw new Exception(responseBody);
                    var responseJson = JsonConvert.DeserializeObject<TokensAccountsByOwnerResponse>(responseBody);
                    if (responseJson?.Result?.Value?.Count > 0)
                        listReturn.Add(responseJson!);
                }
            }
            return listReturn;

        }
    }
}
