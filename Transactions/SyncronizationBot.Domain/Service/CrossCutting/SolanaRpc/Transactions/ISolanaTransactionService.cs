using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.SolanaRpc.Transactions
{
    public interface ISolanaTransactionService
    {
        Task<List<TransactionRPCResponse>?> ExecuteRecoveryTransactionsAsync(TransactionRPCRequest request);
        Task<TransactionRPCDetailResponse> ExecuteRecoveryTransactionDetailAsync(TransactionRPCDetailRequest request);
    }
}
