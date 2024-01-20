using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Jupiter
{
    public interface IJupiterPriceService
    {
        Task<JupiterPricesResponse> ExecuteRecoveryPriceAsync(JupiterPricesRequest request);
    }
}
