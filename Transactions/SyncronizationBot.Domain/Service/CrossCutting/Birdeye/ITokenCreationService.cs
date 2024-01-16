using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenCreation.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenCreation.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Birdeye
{
    public interface ITokenCreationService
    {
        Task<TokenCreationResponse> ExecuteRecoveryTokenCreationAsync(TokenCreationRequest request);
    }
}
