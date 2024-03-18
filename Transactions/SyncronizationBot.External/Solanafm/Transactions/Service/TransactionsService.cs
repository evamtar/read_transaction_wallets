using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Configs;
using System.Collections.Specialized;
using System.Web;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Service
{
    public class TransactionsService : ITransactionsService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TransactionsConfig> _config;
        public TransactionsService(HttpClient httpClient, IOptions<TransactionsConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _config.Value.ApiKey ?? string.Empty);
        }
        public async Task<TransactionsResponse> ExecuteRecoveryTransactionsAsync(TransactionsRequest request)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = request.Page?.ToString();
            query["utcFrom"] = request.UtcFrom.ToString();
            query["utcTo"] = request.UtcTo.ToString();
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.WalletPublicKey) + "?" + query.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return JsonConvert.DeserializeObject<TransactionsResponse>(responseBody) ?? new TransactionsResponse { };
        }
    }
}
