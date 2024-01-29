

namespace SyncronizationBot.Infra.CrossCutting.Solanafm.Tokens.Configs
{
    public class TokensAccountsByOwnerConfig
    {
        public string? BaseUrl { get; set; }
        public List<HeaderConfig>? Headers { get; set; }
        public string? Data { get; set; }
    }

    public class HeaderConfig
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
