using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Birdeye
{
    public interface ITokenSecurityService
    {
        Task<TokenSecurityResponse> ExecuteRecoveryTokenCreationAsync(TokenSecurityRequest request);
    }
}
