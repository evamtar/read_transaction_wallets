

using ReadTransactionsWallets.Domain.Model.Configs;

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferInfo
    {
        private readonly MappedTokensConfig _mappedTokensConfig;
        public TransferInfo(MappedTokensConfig mappedTokensConfig)
        {
            this._mappedTokensConfig = mappedTokensConfig;
        }
        public DateTime? DataOfTransfer { get; set; }
        public TransferToken? TokenSended { get; set; }
        public TransferToken? TokenSendedPool { get; set; }
        public TransferToken? TokenReceived { get; set; }
        public TransferToken? TokenReceivedPool { get; set; }
        public long? PaymentFee { get; set; }
        public ETransactionType TransactionType 
        { 
            get 
            {
                if (this.PaymentFee == null)
                {
                    if (this.TokenSendedPool != null) 
                        return ETransactionType.POOLCREATE;
                    else if (this.TokenReceivedPool != null)
                        return ETransactionType.POOLFINALIZED;
                    else if (this.TokenReceived == null && this.TokenReceived?.Amount > 0)
                        return ETransactionType.RECEIVED;
                    else if (this.TokenReceived == null && this.TokenReceived?.Amount < 0)
                        return ETransactionType.SENDED;
                    return ETransactionType.INDEFINED;
                }
                else 
                {
                    if (this.TokenSendedPool != null)
                        return ETransactionType.POOLCREATE;
                    else if (this.TokenReceivedPool != null)
                        return ETransactionType.POOLFINALIZED;
                    else if (this._mappedTokensConfig.Tokens!.Contains(this.TokenSended?.Token?.Trim() ?? string.Empty))
                        return ETransactionType.BUY;
                    else if (this._mappedTokensConfig.Tokens!.Contains(this.TokenReceived?.Token?.Trim() ?? string.Empty))
                        return ETransactionType.SELL;
                    else if(this.TokenSended != null && this.TokenReceived == null)
                        return ETransactionType.SENDED;
                    return ETransactionType.SWAP;
                }
            }  
        }
    }
}
