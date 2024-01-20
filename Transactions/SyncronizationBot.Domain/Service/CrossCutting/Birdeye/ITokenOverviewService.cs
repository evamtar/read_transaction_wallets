
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenOverview.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Birdeye
{
    public interface ITokenOverviewService
    {
        Task<TokenOverviewResponse> ExecuteRecoveryTokenOverviewAsync(TokenOverviewRequest request);
    }
}
