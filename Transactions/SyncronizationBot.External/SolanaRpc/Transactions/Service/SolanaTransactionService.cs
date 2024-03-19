﻿using Newtonsoft.Json;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Response;
using SyncronizationBot.Domain.Service.CrossCutting.SolanaRpc.Transactions;
using System.Text;

namespace SyncronizationBot.Infra.CrossCutting.SolanaRpc.Transactions.Service
{
    public class SolanaTransactionService : ISolanaTransactionService
    {
        private readonly HttpClient _httpClient;
        

        public SolanaTransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.mainnet-beta.solana.com");
        }

        
        public async Task<List<TransactionRPCResponse>?> ExecuteRecoveryTransactionsAsync(TransactionRPCRequest request)
        {
            var response = (HttpResponseMessage?)null!;
            var responseBody = string.Empty;
            var data = "{\"method\":\"getSignaturesForAddress\",\"params\":[\"{{WalletHash}}\",{\"encoding\":\"jsonParsed\",\"maxSupportedTransactionVersion\":0,\"limit\":1000}],\"jsonrpc\":\"2.0\",\"id\":0}";
            data = data.Replace("{{WalletHash}}", request.WalletHash);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("", content);
            responseBody = await response.Content.ReadAsStringAsync();
            var jsonParsed = JsonConvert.DeserializeObject<TransacionRPC>(responseBody);
            return jsonParsed?.Result?.Where(x => x!.Err == null).Select(x => new TransactionRPCResponse { WalletHash = request.WalletHash, Signature = x!.Signature, BlockTime = x.BlockTime }).ToList();
        }

        public async Task<TransactionRPCDetailResponse> ExecuteRecoveryTransactionDetailAsync(TransactionRPCDetailRequest request)
        {
            var response = (HttpResponseMessage?)null!;
            var responseBody = string.Empty;
            var data = "{ \"method\": \"getTransaction\", \"params\": [ \"{{Signature}}\", { \"encoding\": \"jsonParsed\", \"maxSupportedTransactionVersion\": 0 }], \"jsonrpc\": \"2.0\",\"id\": 1 }";
            data = data.Replace("{{Signature}}", request.Signature);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("", content);
            responseBody = await response.Content.ReadAsStringAsync();
            //var jsonResponse = JsonConvert.DeserializeObject<TransactionDetailRPCResponse>(responseBody);
            return null!;
            //return response.IsSuccessStatusCode ? new TransactionRPCDetailResponse
            //{
            //    IsSuccess = true,
            //    BlockTime = jsonResponse?.Result?.BlockTime,
            //    Fee = jsonResponse?.Result?.Meta?.Fee,
            //    Signature = jsonResponse?.Result?.Transaction?.Signatures?.FirstOrDefault(),
            //    Instructions = this.ConvertInstructionsFromRPC(jsonResponse?.Result?.Meta?.InnerInstructions)
            //} : new TransactionRPCDetailResponse { IsSuccess = false };
        }
    }
}
