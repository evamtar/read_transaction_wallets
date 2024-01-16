using SyncronizationBot.Domain.Model.CrossCutting.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Transactions.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITransactionsService
    {
        Task<TransactionsResponse> ExecuteRecoveryTransactionsAsync(TransactionsRequest request);
    }
}
