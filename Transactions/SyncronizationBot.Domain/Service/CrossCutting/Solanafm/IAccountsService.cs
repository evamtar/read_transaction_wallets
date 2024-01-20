using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface IAccountsService
    {
        Task<AccountsResponse> ExecuteRecoveryAccountAsync(AccountsRequest request);
    }
}
