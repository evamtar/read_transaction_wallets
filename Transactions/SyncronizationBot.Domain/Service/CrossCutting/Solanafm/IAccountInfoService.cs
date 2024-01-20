using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface IAccountInfoService
    {
        Task<AccountInfoResponse> ExecuteRecoveryAccountInfoAsync(AccountInfoRequest request);
    }
}
