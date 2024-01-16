using SyncronizationBot.Domain.Model.CrossCutting.Tokens.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Tokens.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface ITokensService
    {
        Task<TokensResponse> ExecuteRecoveryTokensAsync(TokensRequest request);
    }
}
