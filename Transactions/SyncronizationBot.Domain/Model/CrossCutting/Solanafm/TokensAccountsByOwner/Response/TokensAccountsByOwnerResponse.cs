

namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Response
{
    public class TokensAccountsByOwnerResponse
    {
        public Guid Id { get; set; }
        public string? Jsonrpc { get; set; }
        public TokenResultResponse? Result { get; set; }
    }

    public class TokenResultResponse
    {
        public ContextResponse? Context { get; set; }
        public List<TokenAccountResponse>? Value { get; set; }
    }

    public class ContextResponse
    {
        public string? ApiVersion { get; set; }
        public int Slot { get; set; }
    }

    public class TokenAccountResponse
    {
        public AccountDataResponse? Account { get; set; }
        public string? Pubkey { get; set; }
    }

    public class AccountDataResponse 
    { 
        public AccountDataInfoResponse? Data { get; set; }
    }
    public class AccountDataInfoResponse
    {
        public ParsedDataResponse? Parsed { get; set; }
        public bool Executable { get; set; }
        public long Lamports { get; set; }
        public string? Owner { get; set; }
        public int RentEpoch { get; set; }
        public int Space { get; set; }
    }

    public class ParsedDataResponse
    {
        public TokenInfoResponse? Info { get; set; }
        public string? Type { get; set; }
        public string? Program { get; set; }
        public int Space { get; set; }
    }

    public class TokenInfoResponse
    {
        public bool IsNative { get; set; }
        public string? Mint { get; set; }
        public string? Owner { get; set; }
        public string? State { get; set; }
        public TokenAmountResponse? TokenAmount { get; set; }
    }

    public class TokenAmountResponse
    {
        public decimal? Amount { get; set; }
        public int Decimals { get; set; }
        public int UiAmount { get; set; }
        public string? UiAmountString { get; set; }
    }
}
