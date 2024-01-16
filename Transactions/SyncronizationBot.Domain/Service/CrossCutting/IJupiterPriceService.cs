using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface IJupiterPriceService
    {
        Task<JupiterPricesResponse> ExecuteRecoveryPriceAsync(JupiterPricesRequest request);
    }
}
