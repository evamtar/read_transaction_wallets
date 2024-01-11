

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferManager
    {
        private const string PAYMENT_FEE = "PAYMENT_FEE";
        public Dictionary<string, TransferAccount> Accounts { get; set; } = new Dictionary<string, TransferAccount>();

        public async Task AddTransfer(TransferMapper? transferMapper) 
        {
            var transferAccountSource = this.GetTransferAccount(transferMapper?.Source);
            var transferAccountDestination = this.GetTransferAccount(transferMapper?.Destination);
            switch (transferMapper!.Type)
            {
                
                case ETransferType.PayTxFees:
                     await MakeTransfer(PAYMENT_FEE, transferMapper, transferAccountSource, ETypeOfTransfer.PAYMENT_FEE);
                     break;
                case ETransferType.Transfer:
                case ETransferType.TransferChecked:
                    if (transferMapper.SourceAssociation != null)
                    {    
                        await MakeTransfer(transferMapper.SourceAssociation, transferMapper, transferAccountSource, ETypeOfTransfer.DEBIT);
                        if (transferAccountDestination != null)
                            await MakeTransfer(transferMapper.DestinationAssociation, transferMapper, transferAccountDestination, ETypeOfTransfer.CREDIT);
                    }
                    break;
                case ETransferType.None:
                case ETransferType.CreateAccount:
                case ETransferType.CloseAccount:
                    break;
                default:
                    break;
            }
        }

        private async Task MakeTransfer(string? subAccountHash, TransferMapper? transferMapper, TransferAccount? account, ETypeOfTransfer typeOfTransfer) 
        {
            var subAccount = account?.GetSubAccount(subAccountHash, transferMapper?.DateOfTransfer);
            if (subAccount != null) 
            {
                switch (typeOfTransfer)
                {
                    case ETypeOfTransfer.DEBIT:
                        await subAccount.AddTransferToBalance(this.GetDefaultToken(transferMapper?.Token), (long)((transferMapper?.Amount ?? 0) * -1));
                        break;
                    case ETypeOfTransfer.CREDIT:
                        await subAccount!.AddTransferToBalance(this.GetDefaultToken(transferMapper?.Token), (long)((transferMapper?.Amount ?? 0)));
                        break;
                    case ETypeOfTransfer.PAYMENT_FEE:
                        await subAccount!.AddTransferToBalance(this.GetDefaultToken(transferMapper?.Token), (long)((transferMapper?.Amount ?? 0) * -1));
                        break;
                    default:
                        break;
                }
            }
        }

        private TransferAccount? GetTransferAccount(string? account) 
        {
            if (string.IsNullOrEmpty(account)) return null;
            if (!this.Accounts.ContainsKey(account!))
                this.Accounts.Add(account!, new TransferAccount());
            return this.Accounts[account!];
        }

        private string GetDefaultToken(string? tokenHash) 
        {
            if (string.IsNullOrEmpty(tokenHash))
                return "So11111111111111111111111111111111111111112";
            return tokenHash;
        }
    }
}
