using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Response.SolanaFM.Base
{
    public class TransactionResponse
    {
        public List<TransactionsResponse>? Result { get; set; }
    }

    public class TransactionsResponse
    {
        public decimal? BlockTime { get; set; }
        public string? Signature { get; set; }
        public DateTime DateOfTransaction
        {
            get
            {
                if (BlockTime.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime((long)BlockTime.Value);
                else
                    return DateTime.MinValue;
            }
        }
    }
}
