using SyncronizationBot.Domain.Model.CrossCutting.AccountInfo.Request;
using SyncronizationBot.Domain.Model.CrossCutting.AccountInfo.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting
{
    public interface IAccountInfoService
    {
        Task<AccountInfoResponse> ExecuteRecoveryAccountInfoAsync(AccountInfoRequest request);
    }
}
