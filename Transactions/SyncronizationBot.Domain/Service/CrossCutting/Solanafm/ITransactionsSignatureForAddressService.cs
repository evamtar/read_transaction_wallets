using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response;


namespace SyncronizationBot.Domain.Service.CrossCutting.Solanafm
{
    public interface ITransactionsSignatureForAddressService
    {
        Task<TransactionsSignatureForAddressResponse> ExecuteRecoveryTransactionsForAddressAsync(TransactionsSignatureForAddressRequest request);
    }
}
