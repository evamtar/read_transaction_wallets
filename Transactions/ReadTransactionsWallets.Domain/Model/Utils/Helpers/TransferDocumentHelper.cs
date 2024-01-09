using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;
using ReadTransactionsWallets.Domain.Model.Utils.Transfer;


namespace ReadTransactionsWallets.Domain.Model.Utils.Helpers
{
    public static class TransferDocumentHelper
    {
        public static TransferDocument CreateTransferDocument(List<TransferResponse>? data) 
        {
            TransferDocument document = new TransferDocument();
            document.Root = ConvertToTransferNode(null, data?.FirstOrDefault(x => x.Action == "pay_tx_fees"));
            document.Childrens = CreateChildrens(document.Root, data?.FindAll(x=> x.Action != null && x.Action.StartsWith("transfer")), x=> x.Source == document.Root.Source);
            return document;
        }

        private static TransferNode[]? CreateChildrens(TransferNode? parent, List<TransferResponse>? nodeChildrens, Predicate<TransferResponse> match)
        {
            var transfersChilders = new List<TransferNode> { };
            var childrens = nodeChildrens?.FindAll(match);
            if (childrens != null && childrens.Any())
            {
                nodeChildrens = RemoveActuallyNodes(nodeChildrens, childrens);
                for (int i = 0; i < childrens.Count; i++)
                {
                    TransferNode transferNode = ConvertToTransferNode(parent, childrens[i]);
                    if(nodeChildrens?.Count > 0)
                       transferNode.Childrens = CreateChildrens(transferNode, nodeChildrens, x => x.Source == transferNode?.Destination && x.Destination != transferNode?.Destination);
                    transfersChilders.Add(transferNode);
                }
            }
            return transfersChilders.ToArray();
        }

        private static List<TransferResponse>? RemoveActuallyNodes(List<TransferResponse>? nodeChildrens, List<TransferResponse> childrens)
        {
            childrens?.ForEach(delegate (TransferResponse transfer){
                var finded = nodeChildrens?.Find(f => f.Source == transfer.Source && f.SourceAssociation == transfer.SourceAssociation && f.Destination == transfer.Destination && f.DestinationAssociation == transfer.DestinationAssociation);
                if (finded != null)
                    nodeChildrens!.Remove(finded);
            });
            return nodeChildrens;
        }

        private static TransferNode ConvertToTransferNode(TransferNode? parent, TransferResponse? transferResponse) 
        {
            return new TransferNode
            {
                ParentNode = parent,
                InstructionIndex = transferResponse?.InstructionIndex,
                InnerInstructionIndex = transferResponse?.InnerInstructionIndex,
                Action = transferResponse?.Action,
                Status = transferResponse?.Status,
                Source = transferResponse?.Source,
                SourceAssociation = transferResponse?.SourceAssociation,
                Destination = transferResponse?.Destination,
                DestinationAssociation = transferResponse?.DestinationAssociation,
                Token = transferResponse?.Token,
                Amount = transferResponse?.Amount,
                Timestamp = transferResponse?.Timestamp
            };
        }
    }
}
