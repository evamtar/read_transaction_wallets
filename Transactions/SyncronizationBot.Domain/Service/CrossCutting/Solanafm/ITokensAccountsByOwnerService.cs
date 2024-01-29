using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface ITokensAccountsByOwnerService
    {
        Task<List<TokensAccountsByOwnerResponse>> ExecuteRecoveryTokensAccountsByOwnerAsync(TokensAccountsByOwnerRequest request);
    }
}
