using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request;

namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface ITokensAccountsByOwnerService
    {
        Task<TokensResponse> ExecuteRecoveryTokensAccountsByOwnerAsync(TokensAccountsByOwnerRequest request);
    }
}
