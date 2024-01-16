using SyncronizationBot.Domain.Model.CrossCutting.Accounts.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Accounts.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface IAccountsService
    {
        Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request);
    }
}
