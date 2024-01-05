using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response;

namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface ITransactionsService
    {
        Task<TransactionsResponse> ExecuteRecoveryTransactionsAsync(TransactionsRequest request);
    }
}
