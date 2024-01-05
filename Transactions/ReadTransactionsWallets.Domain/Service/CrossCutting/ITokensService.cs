using ReadTransactionsWallets.Domain.Model.CrossCutting.Tokens.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Tokens.Response;


namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface ITokensService
    {
        Task<TokensResponse> ExecuteRecoveryTokensAsync(TokensRequest request);
    }
}
