

namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Request
{
    public class TokensAccountsByOwnerRequest
    {
        public string? WalletPublicKeyHash { get;set; }
        public Guid? ID { get; set; } = Guid.NewGuid();
    }
}
