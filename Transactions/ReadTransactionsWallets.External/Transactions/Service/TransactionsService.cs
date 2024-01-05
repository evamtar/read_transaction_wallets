
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Transactions.Configs;
using System.Text.Json.Nodes;

namespace ReadTransactionsWallets.Infra.CrossCutting.Transactions.Service
{
    public class TransactionsService : ITransactionsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TransactionsConfig> _config;
        public TransactionsService(HttpClient httpClient, IOptions<TransactionsConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }
        public async Task<TransactionsResponse> ExecuteRecoveryTransactionsAsync(TransactionsRequest request)
        {
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", this._config.Value.ApiKey ?? string.Empty);
            var response = await this._httpClient.GetAsync(string.Format(this._config.Value.ParametersUrl ?? string.Empty, request.WalletPublicKey, request.UtcFrom, request.UtcTo, this._config.Value.Inflow, this._config.Value.Outflow, request.Page));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransactionsResponse>(responseBody) ?? new TransactionsResponse { };
        }
    }
}
