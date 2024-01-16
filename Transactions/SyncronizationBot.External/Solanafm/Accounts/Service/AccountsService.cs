using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Accounts.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Accounts.Response;
using SyncronizationBot.Domain.Service.CrossCutting;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Accounts.Configs;
using System.Text;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Accounts.Service
{
    public class AccountsService : IAccountsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AccountsConfig> _config;
        public AccountsService(HttpClient httpClient, IOptions<AccountsConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _config.Value.ApiKey ?? string.Empty);
        }

        public async Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountsResponse>(responseBody) ?? new AccountsResponse { };
        }
    }
}
