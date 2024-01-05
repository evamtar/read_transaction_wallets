using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;

namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface ITransfersService
    {
        Task<TransfersResponse> ExecuteRecoveryTransfersAsync(TransfersRequest request);
    }
}
