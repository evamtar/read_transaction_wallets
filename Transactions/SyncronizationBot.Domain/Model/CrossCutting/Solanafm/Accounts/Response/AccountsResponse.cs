namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Accounts.Response
{
    public class AccountsResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public List<ResultResponse>? Result { get; set; }
    }

    public class ResultResponse
    {
        public string? AccountHash { get; set; }
        public OnChainResponse? Onchain { get; set; }
    }
    public class OnChainResponse
    {
        public long? Lamports { get; set; }
        public DataResponse? Data { get; set; }
        public string? Owner { get; set; }
        public bool? Executable { get; set; }
        public int? RentEpoch { get; set; }
    }

    public class DataResponse
    {
        public string? Program { get; set; }
        public ParsedResponse? Parsed { get; set; }
        public int? Space { get; set; }
    }
    public class ParsedResponse
    {
        public InfoResponse? Info { get; set; }
        public string? Type { get; set; }
    }
    public class InfoResponse
    {
        public int? Decimals { get; set; }
        public string? FreezeAuthority { get; set; }
        public bool? IsInitialized { get; set; }
        public string? MintAuthority { get; set; }
        public string? Supply { get; set; }
    }
}
