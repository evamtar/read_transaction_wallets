

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferDocument
    {
        public TransferNode? Root { get; set; }
        public TransferNode[]? Childrens { get; set; } = new TransferNode[] { };
        public bool? HasChildren 
        { 
            get 
            { 
                return this.Childrens != null && this.Childrens.Any();
            } 
        }
    }
}
