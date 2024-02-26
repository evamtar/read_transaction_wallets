

namespace SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Transactions
{
    public interface ISolnetTransactionService
    {
        Task ExecuteRecoveryTransactionsAsync();
        Task ExecuteRecoveryTransactionDetailAsync(string signature);
    }
}
