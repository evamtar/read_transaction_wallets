

namespace SyncronizationBot.Domain.Model.Utils.Transfer
{
    public enum ETransferType
    {
        None,
        PayTxFees = 1,
        Transfer = 2,
        TransferChecked = 3,
        CreateAccount = 4,
        CloseAccount = 5
    }
}
