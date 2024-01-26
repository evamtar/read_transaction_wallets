using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Dexscreener.Token.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Dexscreener
{
    public interface IDexScreenerTokenService
    {
        Task<TokenResponse> ExecuteRecoveryTokenAsync(TokenRequest request);
    }
}
