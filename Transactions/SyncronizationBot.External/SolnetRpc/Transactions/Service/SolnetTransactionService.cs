using Polly;
using Polly.Retry;
using Solnet.Extensions;
using Solnet.Rpc;
using SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Transactions;

namespace SyncronizationBot.Infra.CrossCutting.SolnetRpc.Transactions.Service
{
    public class SolnetTransactionService: ISolnetTransactionService
    {
        private readonly IRpcClient _client;
        private readonly RetryPolicy _retryPolicy;

        public SolnetTransactionService()
        {
            this._client = ClientFactory.GetClient(Cluster.MainNet); 
            this._retryPolicy = RetryPolicy.Handle<TokenWalletException>().Or<Exception>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
        }

        public Task ExecuteRecoveryTransactionDetailAsync(string signature)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteRecoveryTransactionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
