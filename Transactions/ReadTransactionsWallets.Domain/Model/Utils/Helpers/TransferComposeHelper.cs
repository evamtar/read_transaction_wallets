using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Model.Utils.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTransactionsWallets.Domain.Model.Utils.Helpers
{
    public static class TransferComposeHelper
    {
        public static List<TransferCompose>? GetTransfersCompose(string? wallet, TransferDocument document, MappedTokensConfig config)
        {
            var transfersCompose = new List<TransferCompose>();
            var rootNode = document.Root;
            if(document.HasChildren ?? false) 
            {
                if (document.Childrens![0].Type == ETransferType.TransferChecked)
                    transfersCompose.Add(CreateTransferCompose(wallet, rootNode!, document.Childrens![0], config));
                else 
                {

                    foreach (var children in document.Childrens)
                    {
                        var firstNode = children;
                        if (!(children.HasChildren ?? true))
                            transfersCompose.Add(CreateTransferCompose(wallet, rootNode!, document.Childrens![0], config));
                        else 
                        {
                            var lastNode = GetLastTransferNode(children.Childrens);
                            transfersCompose.Add(CreateTransferCompose(wallet, firstNode, lastNode, config));
                        }
                    }
                }
            }
            return transfersCompose;
        }

        private static TransferNode GetLastTransferNode(TransferNode[]? childrens)
        {
            if (childrens != null) 
            {
                foreach (var children in childrens)
                {
                    if (children.HasChildren ?? false)
                        return GetLastTransferNode(children.Childrens);
                    else
                        return children;
                }
            }
            return null!;
        }

        private static TransferCompose CreateTransferCompose(string? wallet, TransferNode sendNode, TransferNode lastReceivedNode, MappedTokensConfig config) 
        { 

            return new TransferCompose(transactionWallet: wallet,
                                       transactionSended: ConvertToTransferCompose(sendNode),
                                       transactionReceived: ConvertToTransferCompose(lastReceivedNode),
                                       mappedTokensConfig: config);
        }

        private static TransferComposeData ConvertToTransferCompose(TransferNode transferNode) 
        {
            return new TransferComposeData
            {
                Action = transferNode.Action,
                Status = transferNode.Status,
                Source = transferNode.Source,
                Destination = transferNode.Destination,
                Token = transferNode.Token,
                Amount = transferNode.Amount,
                Timestamp = transferNode.Timestamp
            };
        }
        
    }
}
