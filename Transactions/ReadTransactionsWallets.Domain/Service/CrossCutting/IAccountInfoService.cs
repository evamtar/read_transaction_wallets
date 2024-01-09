using ReadTransactionsWallets.Domain.Model.CrossCutting.AccountInfo.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.AccountInfo.Response;


namespace ReadTransactionsWallets.Domain.Service.CrossCutting
{
    public interface IAccountInfoService
    {
        Task<AccountInfoResponse> ExecuteRecoveryAccountInfoAsync(AccountInfoRequest request);
    }
}
