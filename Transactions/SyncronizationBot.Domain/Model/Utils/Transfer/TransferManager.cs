

using System.Security.Principal;

namespace SyncronizationBot.Domain.Model.Utils.Transfer
{
    public class TransferManager
    {
        private const string PAYMENT_FEE = "PAYMENT_FEE";
        private const string DIRECT_TRANSFER = "DIRECT_TRANSFER";
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
                        await ExistsAndRemoveDuplicateDirectTransfer(DIRECT_TRANSFER, transferMapper, transferAccountSource);
                        await MakeTransfer(transferMapper.SourceAssociation, transferMapper, transferAccountSource, ETypeOfTransfer.DEBIT);
                        if (transferAccountDestination != null)
                            await MakeTransfer(transferMapper.DestinationAssociation, transferMapper, transferAccountDestination, ETypeOfTransfer.CREDIT);
                    }
                    else 
                        await MakeTransfer(DIRECT_TRANSFER, transferMapper, transferAccountSource, ETypeOfTransfer.DEBIT);
                    break;
                case ETransferType.None:
                case ETransferType.CreateAccount:
                case ETransferType.CloseAccount:
                    break;
                default:
                    break;
            }
        }

        private Task ExistsAndRemoveDuplicateDirectTransfer(string? subAccountHash, TransferMapper? transferMapper, TransferAccount? account) 
        {
            if (account?.SubAccounts?.ContainsKey(subAccountHash!) ?? false) 
            {
                var subAccount = account?.SubAccounts?[subAccountHash!];
                if (subAccount?.Balance?.ContainsKey(transferMapper?.Token ?? string.Empty) ?? false) 
                    if (Math.Abs(subAccount?.Balance?[transferMapper?.Token ?? string.Empty] ?? 0) == Math.Abs(transferMapper?.Amount ?? 0))
                        account?.SubAccounts?.Remove(subAccountHash!);
            }
            return Task.CompletedTask;
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
