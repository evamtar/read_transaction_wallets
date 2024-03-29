﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Response;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Infra.CrossCutting.Solanafm.Transfers.Configs;

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Transfers.Service
{
    public class TransfersService : ITransfersService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<TransfersConfig> _config;
        public TransfersService(HttpClient httpClient, IOptions<TransfersConfig> config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config.Value.BaseUrl ?? string.Empty);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _config.Value.ApiKey ?? string.Empty);
        }

        public async Task<TransfersResponse> ExecuteRecoveryTransfersAsync(TransfersRequest request)
        {
            var response = await _httpClient.GetAsync(string.Format(_config.Value.ParametersUrl ?? string.Empty, request.Signature));
            var responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return JsonConvert.DeserializeObject<TransfersResponse>(responseBody) ?? new TransfersResponse { };
        }
    }
}
