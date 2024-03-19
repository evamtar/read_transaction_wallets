

namespace SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Response
{
    public class TransactionRPCDetailResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string? Signature { get; set; }
        public long? BlockTime { get; set; }

        public decimal? Fee { get; set; }
        public decimal? FeeNormalizedAmount
        {
            get
            {
                if (this.Fee.HasValue)
                    return this.Fee / 1000000000;
                return null;
            }
        }
        public List<InstructionResponse>? Instructions { get; set; }
    }

    public class InstructionResponse
    {
        public int? InstructionIndex { get; set; }
        public int? InnerInstructionIndex { get; set; }
        public EActionType? Action { get; set; }
        public string? Source { get; set; }
        public string? SourceAssociation { get; set; }
        public string? Destination { get; set; }
        public string? DestinationAssociation { get; set; }
        public string? Token { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountDecimal { get; set; }
    }

    public enum EActionType
    {
        CreateAccount,
        InitializeAccount,
        Transfer
    }
}
