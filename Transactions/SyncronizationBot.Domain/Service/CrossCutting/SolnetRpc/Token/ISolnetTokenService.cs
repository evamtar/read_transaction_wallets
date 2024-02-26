

using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Response;

namespace SyncronizationBot.Domain.Service.CrossCutting.SolnetRpc.Token
{
    public interface ISolnetTokenService
    {
        Task<TokenRPCResponse> ExecuteRecoveryTokenInfoDetailAsync(TokenRPCRequest request);
    }
}
