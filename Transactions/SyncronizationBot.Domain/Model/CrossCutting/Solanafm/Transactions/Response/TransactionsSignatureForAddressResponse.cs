using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Response
{
    public class TransactionsSignatureForAddressResponse
    {
        public string? Id { get; set; }
        public string? JsonRpc { get; set; }
        public List<TransactionInfoResponse>? Result { get; set; }

    }
    public class TransactionInfoResponse
    {
        public decimal? BlockTime { get; set; }
        public string? ConfirmationStatus { get; set; }
        public string? Signature { get; set; }
        public long Slot { get; set; }
        public InstructionErrorResponse? Err { get; set; }
        public DateTime DateOfTransaction
        {
            get
            {
                if (this.BlockTime.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime((long)this.BlockTime.Value);
                else
                    return DateTime.MinValue;
            }
        }
    }
    public class InstructionErrorResponse
    {
        public List<object>? InstructionError { get; set; }
    }

    public class CustomErrorResponse
    {
        public int? Custom { get; set; }
    }
}
