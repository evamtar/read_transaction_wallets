using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Model.Enum;
using ReadTransactionsWallets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferCompose
    {
        public TransferCompose(string? transactionWallet, 
                               TransferComposeData? transactionSended,
                               TransferComposeData? transactionReceived,
                               MappedTokensConfig? mappedTokensConfig) 
        {
            this.TransactionWallet = transactionWallet;
            this.TransactionSended = transactionSended;
            this.TransactionReceived = transactionReceived;
            this.MappedTokensConfig = mappedTokensConfig;
        }
        public string? TransactionWallet { get; private set; }
        public TransferComposeData? TransactionSended { get; private set; }
        public TransferComposeData? TransactionReceived { get; private set; }
        public MappedTokensConfig? MappedTokensConfig { get; private set; }
        public ETypeOperation TypeOperation { 
            get 
            {
                var operation = ETypeOperation.None;
                switch (this.TransactionReceived!.Type)
                {
                    case ETransferType.None:
                    case ETransferType.PayTxFees:
                    case ETransferType.CreateAccount:
                    case ETransferType.CloseAccount:
                        operation = ETypeOperation.None;
                        break;
                    case ETransferType.TransferChecked:
                        operation = ETypeOperation.Received;
                        break;
                    case ETransferType.Transfer:
                        if (this.MappedTokensConfig!.Tokens!.Contains(this.TransactionSended!.Token!))
                            operation = ETypeOperation.Buy;
                        else if (this.MappedTokensConfig!.Tokens!.Contains(this.TransactionReceived!.Token!))
                            operation = ETypeOperation.Sell;
                        else
                            if(this.TransactionReceived.Destination != TransactionWallet)
                                operation = ETypeOperation.Send;
                            else
                                operation = ETypeOperation.Swap;
                        break;
                    default:
                        break;
                }
                if (operation == ETypeOperation.None) 
                {
                    switch (this.TransactionSended!.Type)
                    {
                        case ETransferType.None:
                        case ETransferType.TransferChecked:
                        case ETransferType.CreateAccount:
                        case ETransferType.CloseAccount:
                            operation = ETypeOperation.None;
                            break;
                        case ETransferType.PayTxFees:
                            operation = ETypeOperation.Received;
                            break;
                        case ETransferType.Transfer:
                            if (this.MappedTokensConfig!.Tokens!.Contains(this.TransactionSended!.Token!))
                                operation = ETypeOperation.Buy;
                            else if (this.MappedTokensConfig!.Tokens!.Contains(this.TransactionReceived!.Token!))
                                operation = ETypeOperation.Sell;
                            else
                                operation = ETypeOperation.Swap;
                            break;
                        default:
                            break;
                    }
                }
                return operation;
            } 
        }
    }

    public class TransferComposeData 
    {
        public string? Action { get; set; }
        public ETransferType Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.Action))
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
        public string? Destination { get; set; }
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
