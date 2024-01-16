using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface IAccountsService
    {
        Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request);
    }
}
