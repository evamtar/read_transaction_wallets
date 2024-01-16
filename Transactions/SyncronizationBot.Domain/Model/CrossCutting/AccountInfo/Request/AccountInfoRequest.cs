

namespace SyncronizationBot.Domain.Model.CrossCutting.AccountInfo.Request
{
    public class AccountInfoRequest
    {
        public string? WalletHash { get; set; }
        public Guid? ID { get; set; } = Guid.NewGuid();
    }
}
