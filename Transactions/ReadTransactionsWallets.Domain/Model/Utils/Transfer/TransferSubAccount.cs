

namespace ReadTransactionsWallets.Domain.Model.Utils.Transfer
{
    public class TransferSubAccount
    {
        public TransferSubAccount(DateTime? dataOfTransfer)
        {
            this.DataOfTransfer = dataOfTransfer;
        }
        public DateTime? DataOfTransfer { get; set; }
        public Dictionary<string, long>? Balance { get; set; } = new Dictionary<string, long>();

        public Task AddTransferToBalance(string tokenHash, long amount) 
        {
            if (!this.Balance!.ContainsKey(tokenHash))
                this.Balance.Add(tokenHash, amount);
            else
                this.Balance[tokenHash] += amount; 
            return Task.CompletedTask;
        }
        
    }
}
