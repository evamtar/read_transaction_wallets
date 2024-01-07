namespace ReadTransactionsWallets.Application.Response
{
    public class RecoverySaveTokenCommandResponse
    {
        public Guid? TokenId { get; set; }
        public int? Decimals { get; set; }
        public string? TokenAlias { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }
        
    }
}
