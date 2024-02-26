
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Response
{
    public class ReadTokenInfoCommandResponse
    {
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public decimal? Supply { get; set; }
        public decimal? Decimals { get; set; }
        public string? MintAuthority { get; set; }
        public string? FreezeAuthority { get; set; }
        public EFontType FontType { get; set; }
        public bool IsSuccess { get; set; }
    }
}
