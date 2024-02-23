using SyncronizationBot.Domain.Model.CrossCutting.Coingecko.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Coingecko.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Coingecko
{
    public interface ICoingeckoTokenService
    {
        Task<TokenResponse> ExecuteRecoveryTokenAsync(TokenRequest request);
    }
}
