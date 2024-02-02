using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Configs;
using System.Text;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Transactions.Service
{
    public class TransactionsSignatureForAddressService: ITransactionsSignatureForAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TransactionsSignatureForAddressConfig> _config;
        public TransactionsSignatureForAddressService(HttpClient httpClient, IOptions<TransactionsSignatureForAddressConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            foreach (var header in this._config.Value.Headers!)
            {
                this._httpClient.DefaultRequestHeaders.Add(header.Key!, header.Value!);
            }
        }

        public async Task<TransactionsSignatureForAddressResponse> ExecuteRecoveryTransactionsForAddressAsync(TransactionsSignatureForAddressRequest request)
        {
            var data = this._config.Value.Data ?? string.Empty;
            data = data.Replace("{{WalletPublicKeyHash}}", request.WalletPublicKeyHash);
            data = data.Replace("{{Id}}", request.ID?.ToString());
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return JsonConvert.DeserializeObject<TransactionsSignatureForAddressResponse>(responseBody) ?? new TransactionsSignatureForAddressResponse { };
        }
    }
}
