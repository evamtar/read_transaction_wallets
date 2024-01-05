namespace ReadTransactionsWallets.Application.Response
{
    public class RecoverySaveTokenCommandResponse
    {
        public int? Decimals { get; set; }
        public string? TokenAlias { get; set; }
        public int Divisor
        {
            get 
            {
                string number = "1";
                for (int i = 0; i < this.Decimals; i++)
                    number += "0";
                return int.Parse(number);
            }
        }
    }
}
