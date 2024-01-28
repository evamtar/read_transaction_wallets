using System.ComponentModel;


namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETelegramChannel
    {
        None = 0,
        [DescriptionAttribute("CallSolanaLog")]
        CallSolanaLog = 1,
        [DescriptionAttribute("CallSolana")]
        CallSolana = 2,
        [DescriptionAttribute("Alert Price Change")]
        AlertPriceChange = 3,
        [DescriptionAttribute("Tokens Alpha")]
        TokenAlpha = 4,
        [DescriptionAttribute("Tokens Info")]
        TokenInfo = 5,
    }

    
}
