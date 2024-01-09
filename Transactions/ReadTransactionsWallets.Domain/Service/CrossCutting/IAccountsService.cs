using ReadTransactionsWallets.Domain.Model.CrossCutting.Accounts.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Accounts.Response;


namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface IAccountsService
    {
        Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request);
    }
}
