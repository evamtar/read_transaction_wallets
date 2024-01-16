namespace SyncronizationBot.Application.Response
{
    public class RecoverySaveTokenCommandResponse
    {
        public Guid? TokenId { get; set; }
        public int? Decimals { get; set; }
        public string? TokenAlias { get; set; }
        public string? TokenHash { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }
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
