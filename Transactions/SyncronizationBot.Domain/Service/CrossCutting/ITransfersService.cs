using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITransfersService
    {
        Task<TransfersResponse> ExecuteRecoveryTransfersAsync(TransfersRequest request);
    }
}
