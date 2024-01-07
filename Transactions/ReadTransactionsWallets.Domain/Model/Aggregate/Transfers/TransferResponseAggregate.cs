using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;
using ReadTransactionsWallets.Domain.Model.Database;

namespace ReadTransactionsWallets.Domain.Model.Aggregate.Transfers
{
    public class TransferResponseAggregate
    {
        public TransferResponse? TransferSource { get; set; }
        public Token? TokenSource { get; set; }
        public int DivisorSource
        {
            get
            {
                string number = "1";
                for (int i = 0; i < this.TokenSource?.Decimals; i++)
                    number += "0";
                return int.Parse(number);
            }
        }
        public TransferResponse? TransferDestination { get; set; }
        public Token? TokenDestination { get; set; }
        public int DivisorDestination
        {
            get
            {
                string number = "1";
                for (int i = 0; i < this.TokenDestination?.Decimals; i++)
                    number += "0";
                return int.Parse(number);
            }
        }
    }
}
