using SyncronizationBot.Domain.Model.CrossCutting.Prices.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Prices.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface IJupiterPriceService
    {
        Task<JupiterPricesResponse> ExecuteRecoveryPriceAsync(JupiterPricesRequest request);
    }
}
