using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Accounts.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Accounts.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Accounts.Configs;
using ReadTransactionsWallets.Infra.CrossCutting.TelegramBot.Configs;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace ReadTransactionsWallets.Infra.CrossCutting.Accounts.Service
{
    public class AccountsService : IAccountsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AccountsConfig> _config;
        public AccountsService(HttpClient httpClient, IOptions<AccountsConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", this._config.Value.ApiKey ?? string.Empty);
        }

        public async Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync("", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountsResponse>(responseBody) ?? new AccountsResponse { };
        }
    }
}
