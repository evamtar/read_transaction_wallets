

using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Response
{
    public class TransactionRPCResponse
    {
        public string? WalletHash { get; set; }
        public string? Signature { get; set; }
        public long? BlockTime { get; set; }
        public DateTime? DateOfTransaction
        {
            get
            {
                if (this.BlockTime.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.BlockTime.Value);
                return null;
            }
        }
    }
}
