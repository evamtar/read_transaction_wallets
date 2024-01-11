using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferAccount
    {
        private const string PAYMENT_FEE = "PAYMENT_FEE";
        public DateTime? DateOfTransfer { get; set; }
        public Dictionary<string, TransferSubAccount>? SubAccounts { get; set; } = new Dictionary<string, TransferSubAccount>();

        public Dictionary<string, long> GetFinalBalance() 
        {
            Dictionary<string, long> finalBalance = new Dictionary<string, long>();
            if (this.SubAccounts != null) 
            {
                foreach (var subaccount in this.SubAccounts) 
                {
                    SetDateOfTransfer(subaccount.Value.DataOfTransfer);
                    if (subaccount.Value.Balance != null) 
                    {
                        foreach (var token in subaccount.Value.Balance) 
                        {
                            if(subaccount.Key == PAYMENT_FEE)
                                finalBalance = AddToBalance(finalBalance, PAYMENT_FEE, token.Value);
                            else
                                finalBalance = AddToBalance(finalBalance, token.Key, token.Value);
                        }
                    }
                }
            }
            return finalBalance;
        }

        public TransferSubAccount? GetSubAccount(string? subAccount, DateTime? dataOfTransfer)
        {
            if (string.IsNullOrEmpty(subAccount)) return null!;
            if (!this.SubAccounts!.ContainsKey(subAccount!))
                this.SubAccounts.Add(subAccount!, new TransferSubAccount(dataOfTransfer));
            return this.SubAccounts[subAccount!];
        }

        private Dictionary<string, long> AddToBalance(Dictionary<string, long> balance, string token, long amountValue) 
        {
            if (amountValue != 0)
            { 
                if (!balance.ContainsKey(token))
                    balance.Add(token, amountValue);
                else
                    balance[token] += amountValue;
            }
            return balance;
        }
        private void SetDateOfTransfer(DateTime? dateOfTransfer) 
        {
            if (this.DateOfTransfer == null)
                this.DateOfTransfer = dateOfTransfer;
        }
    }

    
}
