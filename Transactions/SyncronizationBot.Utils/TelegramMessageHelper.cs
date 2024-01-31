
namespace SyncronizationBot.Utils
{
    public static class TelegramMessageHelper
    {
        private const string MESSAGE_ALERT_PRICE_UP = "<b>*** PRICE UP ***</b>\r\n" +
                                                      "<tg-emoji emoji-id='5368324170671202286'>🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥</tg-emoji>\r\n" +
                                                      "<s>Token Id:</s> {0}\r\n" +
                                                      "<s>Token Name:</s> {1}\r\n" +
                                                      "<s>New Price Change:</s> {2}\r\n" +
                                                      "<s>Is Recurrency Alert:</s> {3}\r\n";
        private const string MESSAGE_ALERT_PRICE_DOWN = "<b>*** PRICE DOWN ***</b>\r\n" +
                                                      "<tg-emoji emoji-id='5368324170671202286'>🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨</tg-emoji>\r\n" +
                                                      "<s>Token Id:</s> {0}\r\n" +
                                                      "<s>Token Name:</s> {1}\r\n" +
                                                      "<s>New Price Change:</s> {2}\r\n" +
                                                      "<s>Is Recurrency Alert:</s> {3}\r\n" +
                                                      "<tg-emoji emoji-id='5368324170671202286'>💸💸💸💸💸💸💸💸💸💸💸</tg-emoji>\r\n";
        public static string GetFormatedMessage(ETypeMessage eTypeMessage, object[] args)
        {
            switch (eTypeMessage)
            {
                case ETypeMessage.PRICE_UP_MESSAGE:
                case ETypeMessage.PRICE_INFO_MESSAGE:
                    return string.Format(MESSAGE_ALERT_PRICE_UP, args);
                case ETypeMessage.PRICE_DOWN_MESSAGE:
                    return string.Format(MESSAGE_ALERT_PRICE_DOWN, args); 
                default:
                    return string.Empty;
            }
        }

        public static Dictionary<string, object> GetParameters(object[]? args) 
        { 
            var parameters = new Dictionary<string, object>();
            if(args != null && args.Any()) 
            {
                foreach (var obj in args)
                {
                    if (obj != null) 
                        parameters.Add(obj?.ToString() ?? string.Empty, obj!);
                }
            }
            return parameters;
        } 
    }

    public enum ETypeMessage 
    { 
        PRICE_UP_MESSAGE = 1,
        PRICE_DOWN_MESSAGE,
        PRICE_INFO_MESSAGE
    }
}
