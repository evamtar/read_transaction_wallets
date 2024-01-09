using ReadTransactionsWallets.Utils;


namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferNode
    {
        public TransferNode? ParentNode { get; set; }
        public TransferNode? NextNode
        {
            get
            {
                return (this.HasChildren ?? false) ? this.Childrens![0] : null;
            }
        }
        public TransferNode[]? Childrens { get; set; } = new TransferNode[] { };
        public bool? HasChildren
        {
            get
            {
                return this.Childrens != null && this.Childrens.Any();
            }
        }
        public int? InstructionIndex { get; set; }
        public int? InnerInstructionIndex { get; set; }
        public string? Action { get; set; }
        public ETransferType Type 
        { 
            get 
            {
                if(string.IsNullOrEmpty(this.Action))
                    return ETransferType.None;
                switch (this.Action.ToLower())
                {
                    case "pay_tx_fees":
                        return ETransferType.PayTxFees;
                    case string s when s.StartsWith("createaccount"):
                        return ETransferType.CreateAccount;
                    case "transfer":
                        return ETransferType.Transfer;
                    case "transferchecked":
                        return ETransferType.TransferChecked;
                    case "closeaccount":
                        return ETransferType.CloseAccount;
                    default:
                        return ETransferType.None;
                }
            } 
        }
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
