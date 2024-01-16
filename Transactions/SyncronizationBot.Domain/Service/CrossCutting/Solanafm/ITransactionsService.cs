using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface ITransactionsService
    {
        Task<TransactionsResponse> ExecuteRecoveryTransactionsAsync(TransactionsRequest request);
    }
}
