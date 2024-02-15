

namespace SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Request
{
    public class SolnetBalanceRequest
    {
        public string? WalletHash { get; set; }
        public bool? IgnoreAmountValueZero { get; set; } = true;
    }
}
