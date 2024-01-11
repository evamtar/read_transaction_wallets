﻿using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;
using ReadTransactionsWallets.Domain.Model.Utils.Transfer;


namespace ReadTransactionsWallets.Domain.Model.Utils.Helpers
{
    public static class TransferManagerHelper
    {
        private const string PAYMENT_FEE = "PAYMENT_FEE";

        public static async Task<TransferManager?> GetTransferManager(List<TransferResponse> transfersResponse)
        {
            TransferManager transferManager = new TransferManager();
            foreach (var transferResponse in transfersResponse)
            {
                var transferMapper = new TransferMapper(transferResponse);
                if(transferMapper != null && transferMapper.Type != ETransferType.CreateAccount && transferMapper.Type != ETransferType.CloseAccount) 
                    await transferManager.AddTransfer(transferMapper);
            }
            return transferManager;
        }

        public static TransferAccount? GetTransferAccount(string? walletSource, string? walletStartTransaction, TransferManager? transferManager)
        {
            return transferManager != null && transferManager.Accounts.ContainsKey(walletSource!) ? transferManager?.Accounts[walletSource!] : transferManager?.Accounts[walletStartTransaction!];
        }

        public static TransferInfo GetTransferInfo(TransferAccount? transferAccount, MappedTokensConfig mappedTokensConfig) 
        {
            var transferInfo = new TransferInfo(mappedTokensConfig);
            transferInfo.DataOfTransfer = transferAccount?.DateOfTransfer;
            var finalBalance = transferAccount?.GetFinalBalance();
            if ((finalBalance != null && finalBalance.ContainsKey(PAYMENT_FEE) && finalBalance.Keys.Count > 3) || (finalBalance != null && !finalBalance.ContainsKey(PAYMENT_FEE) && finalBalance.Keys.Count > 2))
                throw new Exception("Mapped nethod needs verify");
            else 
            {
                TransferToken tokenSended = new TransferToken();
                TransferToken tokenReceived = new TransferToken();
                if (finalBalance != null) 
                {
                    foreach (var balance in finalBalance)
                    {
                        if (balance.Key == PAYMENT_FEE)
                            transferInfo.PaymentFee = balance.Value;
                        else
                        {
                            if (tokenSended.Amount == 0 && balance.Value < 0)
                            {
                                tokenSended.Token = balance.Key;
                                tokenSended.Amount = balance.Value;
                                transferInfo.TokenSended = tokenSended;
                            }
                            else if (tokenReceived.Amount == 0 && balance.Value > 0)
                            {
                                tokenReceived.Token = balance.Key;
                                tokenReceived.Amount = balance.Value;
                                transferInfo.TokenReceived = tokenReceived;
                            }
                            else
                                throw new Exception("Mapping has problem. Verify!");
                        }
                    }
                }
            }
            return transferInfo;
        }

    }
}
