using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Birdeye
{
    public interface IWalletPortifolioService
    {
        Task<WalletPortifolioResponse> ExecuteRecoveryWalletPortifolioAsync(WalletPortifolioRequest request);
    }
}
