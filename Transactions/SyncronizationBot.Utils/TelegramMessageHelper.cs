
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
        private const string MESSAGE_LOG_EXECUTE_ERROR = "<b>O serviço {2} suspendeu a execução.</b>\r\n" +
                                                         "<i><b>Mensagem de erro:</b> {1}</i>.\r\n" +
                                                         "StackTrace: {2}\r\n" +
                                                         "<i><b>Proxima execução</b> no período timer de --> {3}. \r\n" +
                                                         "<b>Dev's Favor verificar</b> Cc:@evandrotartari , @euRodrigo</i>";
        private const string MESSAGE_BUY_MESSAGE_HEADER = "<b>*** NEW BUY ALERT ***</b>\r\n";
        private const string MESSAGE_BUY_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>\r\n";
        private const string MESSAGE_BUY_MESSAGE_BODY   = "<i>Signature:</i> {0}\r\n" +
                                                          "<i>WalletHash:</i> {1}\r\n" +
                                                          "<i>ClassWallet:</i> {2} \r\n" +
                                                          "<i>Token:</i> {3}\r\n" +
                                                          "<i>Ca:</i> {4}\r\n" +
                                                          "<i>Minth Authority:</i> {5}\r\n" +
                                                          "<i>Freeze Authority:</i> {6}\r\n" +
                                                          "<i>Is Mutable:</i> {7}\r\n" +
                                                          "<i>Quantity:</i> {8}\r\n" +
                                                          "<i>Value Spent:</i> {9}\r\n" +
                                                          "<i>Date:</i> {10}}\r\n";
        private const string MESSAGE_BUY_CHART = "<a href=\"https://birdeye.so/token/{11}?chain=solana\">Chart</a>";
        private const string MESSAGE_REBUY_MESSAGE_HEADER = "<b>*** NEW REBUY ALERT ***</b>\r\n";
        private const string MESSAGE_REBUY_MESSAGE_ICONS = "<tg-emoji emoji-id='5368324170671202286'>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>\r\n";
        private const string MESSAGE_SELL_MESSAGE_HEADER = "<b>*** NEW SELL ALERT ***</b>\r\n";
        private const string MESSAGE_SELL_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>\r\n";
        private const string MESSAGE_SELL_MESSAGE_BODY   = "<i>Signature:</i> {0}\r\n" +
                                                           "<i>WalletHash:</i> {1}\r\n" +
                                                           "<i>ClassWallet:</i> {2} \r\n" +
                                                           "<i>Token:</i> {3}\r\n" +
                                                           "<i>Quantity:</i> {4}\r\n" +
                                                           "<i>Value Received:</i> {5}\r\n" +
                                                           "<i>Date:</i> {6}}\r\n";
        private const string MESSAGE_SELL_CHART = "<a href=\"https://birdeye.so/token/{7}?chain=solana\"> Chart</a>";
        private const string MESSAGE_SWAP_MESSAGE_HEADER = "<b>*** SWAP ALERT ***</b>\r\n";
        private const string MESSAGE_SWAP_MESSAGE_ICONS  = "<tg-emoji emoji-id='5368324170671202286'>🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄🔄</tg-emoji>\r\n";
        private const string MESSAGE_SWAP_MESSAGE_BODY = "<i>Signature:</i> {0}\r\n" +
                                                         "<i>WalletHash:</i> {1}\r\n" +
                                                         "<i>ClassWallet:</i> {2} \r\n" +
                                                         "<i>Token Change:</i> {3}\r\n" +
                                                         "<i>Token Received:</i> {4}\r\n" +
                                                         "<i>Ca:</i> {5}\r\n" +
                                                         "<i>Date:</i> {6}}\r\n";
        private const string MESSAGE_SWAP_CHART_1 = "<a href=\"https://birdeye.so/token/{7}?chain=solana\"> Chart1</a>}\r\n";
        private const string MESSAGE_SWAP_CHART_2 = "<a href=\"https://birdeye.so/token/{8}?chain=solana\"> Chart2</a>";
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
                                                            "<i>Signature:</i> {0}\r\n" +
                                                            "<i>WalletHash:</i> {1}\r\n" +
                                                            "<i>ClassWallet:</i> {2} \r\n" +
                                                            "<i>Amount Pool:</i> {3}\r\n" +
                                                            "<i>Amount Pool:</i> {4}\r\n" +
                                                            "<i>Ca Token Pool:</i> {5}\r\n" +
                                                            "<i>Ca Token Pool:</i> {6}\r\n" +
                                                            "<i>Date:</i> {7}\r\n";
        private const string MESSAGE_POOL_CREATED_CHART_1 = "<a href=\"https://birdeye.so/token/{8}?chain=solana\"> Chart1</a>}\r\n";
        private const string MESSAGE_POOL_CREATED_CHART_2 = "<a href=\"https://birdeye.so/token/{9}?chain=solana\"> Chart2</a>";
        private const string MESSAGE_POOL_FINALIZED_MESSAGE = "<b>*** POOL FINALIZED ***</b>\r\n" +
                                                              "<tg-emoji emoji-id='5368324170671202286'>❌❌❌❌❌❌❌❌❌❌❌</tg-emoji>\r\n" +
                                                              "<i>Signature:</i> {0}\r\n" +
                                                              "<i>WalletHash:</i> {1}\r\n" +
                                                              "<i>ClassWallet:</i> {2} \r\n" +
                                                              "<i>Amount Pool:</i> {3}\r\n" +
                                                              "<i>Amount Pool:</i> {4}\r\n" +
                                                              "<i>Ca Token Pool:</i> {5}\r\n" +
                                                              "<i>Ca Token Pool:</i> {6}\r\n" +
                                                              "<i>Date:</i> {7}\r\n";
        private const string MESSAGE_POOL_FINALIZED_CHART_1 = "<a href=\"https://birdeye.so/token/{8}?chain=solana\"> Chart1</a>}\r\n";
        private const string MESSAGE_POOL_FINALIZED_CHART_2 = "<a href=\"https://birdeye.so/token/{9}?chain=solana\"> Chart2</a>";
        private const string MESSAGE_ALERT_PRICE_UP = "<b>*** PRICE UP ***</b>\r\n" +
                                                      "<tg-emoji emoji-id='5368324170671202286'>🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥</tg-emoji>\r\n" +
                                                      "<i>Token Id:</i> {0}\r\n" +
                                                      "<i>Token Name:</i> {1}\r\n" +
                                                      "<i>New Price Change:</i> {2}\r\n" +
                                                      "<i>Is Recurrency Alert:</i> {3}\r\n";
        private const string MESSAGE_ALERT_PRICE_DOWN = "<b>*** PRICE DOWN ***</b>\r\n" +
                                                      "<tg-emoji emoji-id='5368324170671202286'>🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨🚨</tg-emoji>\r\n" +
                                                      "<i>Token Id:</i> {0}\r\n" +
                                                      "<i>Token Name:</i> {1}\r\n" +
                                                      "<i>New Price Change:</i> {2}\r\n" +
                                                      "<i>Is Recurrency Alert:</i> {3}\r\n" +
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
