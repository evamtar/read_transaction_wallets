using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Configs;
using System.Text;


namespace SyncronizationBot.Infra.CrossCutting.Solanafm.AccountInfo.Service
{
    public class AccountInfoService : IAccountInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AccountInfoConfig> _config;
        public AccountInfoService(HttpClient httpClient, IOptions<AccountInfoConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            foreach (var header in this._config.Value.Headers!) 
            {
                this._httpClient.DefaultRequestHeaders.Add(header.Key!, header.Value!);
            }
        }

        public async Task<AccountInfoResponse> ExecuteRecoveryAccountInfoAsync(AccountInfoRequest request)
        {
            var data = this._config.Value.Data ?? string.Empty;
            data = data.Replace("{{accountHash}}", request.WalletHash);
            data = data.Replace("{{Id}}", request.ID?.ToString());
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return JsonConvert.DeserializeObject<AccountInfoResponse>(responseBody) ?? new AccountInfoResponse { };
        }
    }
}
