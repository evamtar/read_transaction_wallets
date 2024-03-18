using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Models;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Response;
using SyncronizationBot.Domain.Service.CrossCutting.SolanaRpc.Transactions;

namespace SyncronizationBot.Infra.CrossCutting.SolanaRpc.Transactions.Service
{
    public class SolanaTransactionService : ISolanaTransactionService
    {
        private readonly IRpcClient _client;
        private readonly HttpClient _httpClient;
        private readonly RetryPolicy _retryPolicy;


        public SolanaTransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _client = ClientFactory.GetClient(Cluster.MainNet);
            _retryPolicy = Policy.Handle<Exception>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { Console.WriteLine(ex); });
            _httpClient.BaseAddress = new Uri("https://api.mainnet-beta.solana.com");
        }

        
        public async Task<List<TransactionRPCResponse>?> ExecuteRecoveryTransactionsAsync(TransactionRPCRequest request)
        {
            var result = (RequestResult<List<SignatureStatusInfo>>)null!;
            await _retryPolicy.Execute(async () =>
            {
                result = await _client.GetSignaturesForAddressAsync(request.WalletHash, limit:200);
            });
            return result.WasSuccessful ? result.Result.Select(x => new TransactionRPCResponse { WalletHash = request.WalletHash, Signature = x.Signature, BlockTime = (long?)x.BlockTime }).ToList() : null;
        }
        
    }
}
