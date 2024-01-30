

namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request
{
    public class TransactionsSignatureForAddressRequest
    {
        public string? WalletPublicKeyHash { get; set; }
        public Guid? ID { get; set; } = Guid.NewGuid();
    }
}
