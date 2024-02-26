

using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token.Response
{
    public class TokenRPCResponse
    {
        public decimal? Supply {  get; set; }
        public int? Decimals { get; set; }
        public string? MintAuthority { get; set; }
        public string? FreezeAuthority { get; set; }
        public EFontType FontType { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
