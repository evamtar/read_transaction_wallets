
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Transactions.Configs;
using System.Collections.Specialized;
using System.Text.Json.Nodes;
using System.Web;

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
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["utcFrom"] = request.UtcFrom.ToString();
            query["utcTo"] = request.UtcTo.ToString();
            query["inflow"] = _config.Value.Inflow?.ToString().ToLower();
            query["outflow"] = _config.Value.Outflow?.ToString().ToLower();
            query["page"] = request.Page?.ToString();
            var response = await this._httpClient.GetAsync(string.Format(this._config.Value.ParametersUrl ?? string.Empty, request.WalletPublicKey) + "?" + query.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransactionsResponse>(responseBody) ?? new TransactionsResponse { };
        }
    }
}
