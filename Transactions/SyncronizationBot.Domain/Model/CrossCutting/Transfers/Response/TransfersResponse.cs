
using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.CrossCutting.Transfers.Response
{
    public class TransfersResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public ResultResponse? Result { get; set; }
    }

    public class ResultResponse
    {
        public string? TransactionHash { get; set; }
        public List<TransferResponse>? Data { get; set; }
    }

    public class TransferResponse 
    {
        public int? InstructionIndex { get; set; }
        public int? InnerInstructionIndex { get; set; }
        public string? Action { get; set; }
        public string? Status { get; set; }
        public string? Source { get; set; }
        public string? SourceAssociation { get; set; }
        public string? Destination { get; set; }
        public string? DestinationAssociation { get; set; }
        public string? Token { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Timestamp { get; set; }
        public DateTime DateOfTransfer
        {
            get 
            {
                if (this.Timestamp.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime((long)this.Timestamp.Value);
                else
                    return DateTime.MinValue;
            }
        }
    }
}
