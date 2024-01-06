

namespace ReadTransactionsWallets.Infra.CrossCutting.TelegramBot.Configs
{
    public class TelegramBotConfig
    {
        public string? BaseUrl { get; set; }
        public string? ParametersUrlPostChannel { get; set; }
        public string? ParametersUrlSendMessage { get; set; }
        public string? Token { get; set; }
    }
}
