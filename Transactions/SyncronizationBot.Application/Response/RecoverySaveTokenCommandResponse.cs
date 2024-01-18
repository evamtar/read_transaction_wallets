namespace SyncronizationBot.Application.Response
{
    public class RecoverySaveTokenCommandResponse
    {
        public Guid? TokenId { get; set; }
        public string? Hash { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public decimal? Supply { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Liquidity { get; set; }
        public int? UniqueWallet24h { get; set; }
        public int? UniqueWalletHistory24h { get; set; }
        public int? Decimals { get; set; }
        public int? NumberMarkets { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }
        public DateTime? DateCreation { get; set; }
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
