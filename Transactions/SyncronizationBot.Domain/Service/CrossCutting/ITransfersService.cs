using SyncronizationBot.Domain.Model.CrossCutting.Transfers.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Transfers.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITransfersService
    {
        Task<TransfersResponse> ExecuteRecoveryTransfersAsync(TransfersRequest request);
    }
}
