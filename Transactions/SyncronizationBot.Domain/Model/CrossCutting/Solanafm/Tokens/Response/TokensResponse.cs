namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Tokens.Response
{
    public class TokensResponse
    {
        public string? Mint { get; set; }
        public int? Decimals { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        public string? TokenType { get; set; }
        public TokenListResponse? TokenList { get; set; }
        public TokenMetadataResponse? TokenMetadata { get; set; }
    }

    public class TokenListResponse
    {

        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public string? Image { get; set; }
        public int? ChainId { get; set; }
    }

    public class TokenMetadataResponse
    {
        public OnChainInfoResponse? OnChainInfo { get; set; }
    }

    public class OnChainInfoResponse
    {
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public string? Metadata { get; set; }
        public string? UpdateAuthority { get; set; }
        public bool? IsMasterEdition { get; set; }
        public string? Edition { get; set; }
        public string? Uri { get; set; }
        public int? SellerFeeBasisPoints { get; set; }
        public bool? PrimarySaleHappened { get; set; }
        public bool? IsMutable { get; set; }
    }
}
