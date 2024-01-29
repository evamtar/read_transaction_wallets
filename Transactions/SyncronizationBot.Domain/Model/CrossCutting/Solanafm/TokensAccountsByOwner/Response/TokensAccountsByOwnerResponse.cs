

namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.TokensAccountsByOwner.Response
{
    public class TokensAccountsByOwnerResponse
    {
        public Guid Id { get; set; }
        public string? Jsonrpc { get; set; }
        public TokenResult? Result { get; set; }
    }

    public class TokenResult
    {
        public Context? Context { get; set; }
        public List<TokenAccount>? Value { get; set; }
    }

    public class Context
    {
        public string? ApiVersion { get; set; }
        public int Slot { get; set; }
    }

    public class TokenAccount
    {
        public AccountData? Account { get; set; }
        public string? Pubkey { get; set; }
    }

    public class AccountData
    {
        public ParsedData? Parsed { get; set; }
        public bool Executable { get; set; }
        public long Lamports { get; set; }
        public string? Owner { get; set; }
        public int RentEpoch { get; set; }
        public int Space { get; set; }
    }

    public class ParsedData
    {
        public TokenInfo? Info { get; set; }
        public string? Type { get; set; }
        public string? Program { get; set; }
        public int Space { get; set; }
    }

    public class TokenInfo
    {
        public bool IsNative { get; set; }
        public string? Mint { get; set; }
        public string? Owner { get; set; }
        public string? State { get; set; }
        public TokenAmount? TokenAmount { get; set; }
    }

    public class TokenAmount
    {
        public string? Amount { get; set; }
        public int Decimals { get; set; }
        public int UiAmount { get; set; }
        public string? UiAmountString { get; set; }
    }
}
