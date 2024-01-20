using System.ComponentModel;


namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETelegramChannel
    {
        [DescriptionAttribute("CallSolanaLog")]
        CallSolanaLog = 1,
        [DescriptionAttribute("CallSolana")]
        CallSolana = 2,
        [DescriptionAttribute("Alert Price Change")]
        AlertPriceChange = 3
    }

    
}
