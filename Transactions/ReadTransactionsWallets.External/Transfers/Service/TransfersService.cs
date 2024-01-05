using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Infra.CrossCutting.Transfers.Configs;

namespace ReadTransactionsWallets.Infra.CrossCutting.Transfers.Service
{
    public class TransfersService : ITransfersService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TransfersConfig> _config;
        public TransfersService(HttpClient httpClient, IOptions<TransfersConfig> config)
        {
            this._httpClient = httpClient;
            this._config = config;
            this._httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
        }

        public async Task<TransfersResponse> ExecuteRecoveryTransfersAsync(TransfersRequest request)
        {
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", this._config.Value.ApiKey ?? string.Empty);
            var response = await this._httpClient.GetAsync(string.Format(this._config.Value.ParametersUrl ?? string.Empty, request.Signature));
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransfersResponse>(responseBody) ?? new TransfersResponse { };
        }
    }
}
