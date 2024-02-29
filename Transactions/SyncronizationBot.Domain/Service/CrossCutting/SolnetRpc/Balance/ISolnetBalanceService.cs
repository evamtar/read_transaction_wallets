using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Balance
{
    public interface ISolnetBalanceService
    {
        Task<SolnetBalanceResponse> ExecuteRecoveryWalletBalanceAsync(SolnetBalanceRequest request);
    }
}
