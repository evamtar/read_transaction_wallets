
namespace SyncronizationBot.Utils
{
    public static class TelegramMessageHelper
    {
        private const string MESSAGE_LOG_EXECUTE = "<b>Execução do serviço {0} de call solana</b>\r\n" +
                                                   "<b>Data Execução: </b>{1}.\r\n" +
                                                   "<i><b>Proxima execução</b> no período timer de --> {2}</i>";
        private const string MESSAGE_LOG_APP_RUNNING = "<b>O serviço {0} está rodando.</b>\r\n" +
                                                       "<i><b>Não irá efetuar essa execução:</b> {1}</i>.\r\n";
        private const string MESSAGE_LOG_APP_TIME_NULL = "<b>Timer do serviço {0} está nulo ou não configurado.</b>\r\n" +
                                                         "<i><b>Não irá efetuar essa execução:</b> {1}</i>.\r\n";
        private const string MESSAGE_LOG_EXECUTE_ERROR = "<b>O serviço {0} suspendeu a execução.</b>\r\n" +
                                                         "<i><b>Mensagem de erro:</b> {1}</i>.\r\n" +
                                                         "StackTrace: {2}\r\n" +
                                                         "<i><b>Proxima execução</b> no período timer de --> {3}. \r\n" +
                                                         "<b>Dev's Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>";
        private const string MESSAGE_BUY_MESSAGE_HEADER = "<b>*** NEW BUY ALERT ***</b>\r\n";
        private const string MESSAGE_BUY_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>\r\n";
        private const string MESSAGE_BUY_MESSAGE_BODY   = "<s>Signature:</s> {0}\r\n" +
                                                          "<s>WalletHash:</s> {1}\r\n" +
                                                          "<s>ClassWallet:</s> {2} \r\n" +
                                                          "<s>Token:</s> {3}\r\n" +
                                                          "<s>Ca:</s> {4}\r\n" +
                                                          "<s>Minth Authority:</s> {5}\r\n" +
                                                          "<s>Freeze Authority:</s> {6}\r\n" +
                                                          "<s>Is Mutable:</s> {7}\r\n" +
                                                          "<s>Quantity:</s> {8}\r\n" +
                                                          "<S>Value Spent:</s> {9}\r\n" +
                                                          "<s>Date:</s> {10}\r\n" + 
                                                          "<s>Position Increase</s> {11}\r\n";
        private const string MESSAGE_BUY_CHART = "<a href='https://birdeye.so/token/{12}?chain=solana'>Chart</a>";
        private const string MESSAGE_REBUY_MESSAGE_HEADER = "<b>*** NEW REBUY ALERT ***</b>\r\n";
        private const string MESSAGE_REBUY_MESSAGE_ICONS = "<tg-emoji emoji-id='5368324170671202286'>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>\r\n";
        private const string MESSAGE_SELL_MESSAGE_HEADER = "<b>*** NEW SELL ALERT ***</b>\r\n";
        private const string MESSAGE_SELL_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>\r\n";
        private const string MESSAGE_SELL_MESSAGE_BODY   = "<s>Signature:</s> {0}\r\n" +
                                                           "<s>WalletHash:</s> {1}\r\n" +
                                                           "<s>ClassWallet:</s> {2} \r\n" +
                                                           "<s>Token:</s> {3}\r\n" +
                                                           "<s>Quantity:</s> {4}\r\n" +
                                                           "<s>Value Received:</s> {5}\r\n" +
                                                           "<s>Date:</s> {6}\r\n" +
                                                           "<s>Position Sell:</s> {7}\r\n";
        private const string MESSAGE_SELL_CHART = "<a href='https://birdeye.so/token/{8}?chain=solana'>Chart</a>";
        private const string MESSAGE_SWAP_MESSAGE_HEADER = "<b>*** SWAP ALERT ***</b>\r\n";
        private const string MESSAGE_SWAP_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄</tg-emoji>\r\n";
        private const string MESSAGE_SWAP_MESSAGE_BODY = "<s>Signature:</s> {0}\r\n" +
                                                         "<s>WalletHash:</s> {1}\r\n" +
                                                         "<s>ClassWallet:</s> {2} \r\n" +
                                                         "<s>Token Change:</s> {3}\r\n" +
                                                         "<s>Token Received:</s> {4}\r\n" +
                                                         "<s>Ca:</s> {5}\r\n" +
                                                         "<s>Date:</s> {6}\r\n" +
                                                         "<s>Position Swap:</s> {7}\r\n";
        private const string MESSAGE_SWAP_CHART_1 = "<a href='https://birdeye.so/token/{8}?chain=solana'>Chart1</a>\r\n";
        private const string MESSAGE_SWAP_CHART_2 = "<a href='https://birdeye.so/token/{9}?chain=solana'>Chart2</a>";
        private const string MESSAGE_MM_NEW_BUY_MESSAGE_HEADER = "<b>*** NEW MM BUY ALERT ***</b>\r\n";
        private const string MESSAGE_MM_NEW_BUY_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>\r\n";
        private const string MESSAGE_MM_FOOTER                 = "Cc:@evandrotartari , @euRodrigo, @xton_eth";
        private const string MESSAGE_MM_REBUY_MESSAGE_HEADER   = "<b>*** NEW MM REBUY ALERT ***</b>\r\n";
        private const string MESSAGE_MM_REBUY_MESSAGE_ICONS    = "<tg-emoji emoji-id='5368324170671202286'>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>\r\n";
        private const string MESSAGE_MM_SELL_MESSAGE_HEADER = "<b>*** NEW MM SELL ALERT ***</b>\r\n";
        private const string MESSAGE_MM_SELL_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>\r\n";
        private const string MESSAGE_MM_SWAP_MESSAGE_HEADER = "<b>*** SWAP MM ALERT ***</b>\r\n";
        private const string MESSAGE_POOL_CREATED_MESSAGE = "<b>*** POOL CREATED ***</b>\r\n" +
                                                            "<tg-emoji emoji-id='5368324170671202286'>🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊</tg-emoji>\r\n" +
                                                            "<s>Signature:</s> {0}\r\n" +
                                                            "<s>WalletHash:</s> {1}\r\n" +
                                                            "<s>ClassWallet:</s> {2} \r\n" +
                                                            "<s>Amount Pool:</s> {3}\r\n" +
                                                            "<s>Amount Pool:</s> {4}\r\n" +
                                                            "<s>Ca Token Pool:</s> {5}\r\n" +
                                                            "<s>Ca Token Pool:</s> {6}\r\n" +
                                                            "<s>Date:</s> {7}\r\n";
        private const string MESSAGE_POOL_CREATED_CHART_1 = "<a href='https://birdeye.so/token/{8}?chain=solana'> Chart1</a>}\r\n";
        private const string MESSAGE_POOL_CREATED_CHART_2 = "<a href='https://birdeye.so/token/{9}?chain=solana'> Chart2</a>";
        private const string MESSAGE_POOL_FINALIZED_MESSAGE = "<b>*** POOL FINALIZED ***</b>\r\n" +
                                                              "<tg-emoji emoji-id='5368324170671202286'>❌❌❌❌❌❌❌❌❌❌❌</tg-emoji>\r\n" +
                                                              "<s>Signature:</s> {0}\r\n" +
                                                              "<s>WalletHash:</s> {1}\r\n" +
                                                              "<s>ClassWallet:</s> {2} \r\n" +
                                                              "<s>Amount Pool:</s> {3}\r\n" +
                                                              "<s>Amount Pool:</s> {4}\r\n" +
                                                              "<s>Ca Token Pool:</s> {5}\r\n" +
                                                              "<s>Ca Token Pool:</s> {6}\r\n" +
                                                              "<s>Date:</s> {7}\r\n";
        private const string MESSAGE_POOL_FINALIZED_CHART_1 = "<a href='https://birdeye.so/token/{8}?chain=solana'>Chart1</a>\r\n";
        private const string MESSAGE_POOL_FINALIZED_CHART_2 = "<a href='https://birdeye.so/token/{9}?chain=solana'>Chart2</a>";
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
                case ETypeMessage.LOG_EXECUTE:
                    return string.Format(MESSAGE_LOG_EXECUTE, args);
                case ETypeMessage.LOG_APP_RUNNING:
                    return string.Format(MESSAGE_LOG_APP_RUNNING, args);
                case ETypeMessage.LOG_APP_TIME_NULL:
                    return string.Format(MESSAGE_LOG_APP_TIME_NULL, args);
                case ETypeMessage.LOG_EXECUTE_ERROR:
                    return string.Format(MESSAGE_LOG_EXECUTE_ERROR, args);
                case ETypeMessage.BUY_MESSAGE:
                    return string.Format(MESSAGE_BUY_MESSAGE_HEADER + MESSAGE_BUY_MESSAGE_ICONS + MESSAGE_BUY_MESSAGE_BODY + MESSAGE_BUY_CHART, args);
                case ETypeMessage.REBUY_MESSAGE:
                    return string.Format(MESSAGE_REBUY_MESSAGE_HEADER + MESSAGE_REBUY_MESSAGE_ICONS + MESSAGE_BUY_MESSAGE_BODY + MESSAGE_BUY_CHART, args);
                case ETypeMessage.SELL_MESSAGE:
                    return string.Format(MESSAGE_SELL_MESSAGE_HEADER + MESSAGE_SELL_MESSAGE_ICONS + MESSAGE_SELL_MESSAGE_BODY + MESSAGE_SELL_CHART, args);
                case ETypeMessage.SWAP_MESSAGE:
                    return string.Format(MESSAGE_SWAP_MESSAGE_HEADER + MESSAGE_SWAP_MESSAGE_ICONS + MESSAGE_SWAP_MESSAGE_BODY + MESSAGE_SWAP_CHART_1 + MESSAGE_SWAP_CHART_2, args); 
                case ETypeMessage.MM_NEW_BUY_MESSAGE:
                    return string.Format(MESSAGE_MM_NEW_BUY_MESSAGE_HEADER + MESSAGE_MM_NEW_BUY_MESSAGE_ICONS + MESSAGE_BUY_MESSAGE_BODY + MESSAGE_BUY_CHART + MESSAGE_MM_FOOTER, args);
                case ETypeMessage.MM_REBUY_MESSAGE:
                    return string.Format(MESSAGE_MM_REBUY_MESSAGE_HEADER + MESSAGE_MM_REBUY_MESSAGE_ICONS + MESSAGE_BUY_MESSAGE_BODY + MESSAGE_BUY_CHART, args);
                case ETypeMessage.MM_SELL_MESSAGE:
                    return string.Format(MESSAGE_MM_SELL_MESSAGE_HEADER + MESSAGE_MM_SELL_MESSAGE_ICONS + MESSAGE_SELL_MESSAGE_BODY + MESSAGE_SELL_CHART + MESSAGE_MM_FOOTER, args);
                case ETypeMessage.MM_SWAP_MESSAGE:
                    return string.Format(MESSAGE_MM_SWAP_MESSAGE_HEADER + MESSAGE_SWAP_MESSAGE_ICONS + MESSAGE_SWAP_MESSAGE_BODY + MESSAGE_SWAP_CHART_1 + MESSAGE_SWAP_CHART_2 + MESSAGE_MM_FOOTER, args);
                case ETypeMessage.POOL_CREATED_MESSAGE:
                    return string.Format(MESSAGE_POOL_CREATED_MESSAGE + MESSAGE_POOL_CREATED_CHART_1 + MESSAGE_POOL_CREATED_CHART_2, args);
                case ETypeMessage.POOL_FINALIZED_MESSAGE:
                    return string.Format(MESSAGE_POOL_FINALIZED_MESSAGE + MESSAGE_POOL_FINALIZED_CHART_1 + MESSAGE_POOL_FINALIZED_CHART_2, args);
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
        LOG_EXECUTE = 1,
        LOG_APP_RUNNING,
        LOG_APP_TIME_NULL,
        LOG_EXECUTE_ERROR,
        BUY_MESSAGE,
        REBUY_MESSAGE,
        SELL_MESSAGE,
        SWAP_MESSAGE,
        MM_NEW_BUY_MESSAGE,
        MM_REBUY_MESSAGE,
        MM_SELL_MESSAGE,
        MM_SWAP_MESSAGE,
        POOL_CREATED_MESSAGE,
        POOL_FINALIZED_MESSAGE,
        PRICE_UP_MESSAGE,
        PRICE_DOWN_MESSAGE,
        PRICE_INFO_MESSAGE
    }
}
