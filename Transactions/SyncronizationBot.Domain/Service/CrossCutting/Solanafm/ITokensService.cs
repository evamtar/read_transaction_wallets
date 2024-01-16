using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface ITokensService
    {
        Task<TokensResponse> ExecuteRecoveryTokensAsync(TokensRequest request);
    }
}
