
namespace ReadTransactionsWallets.Utils
{
    public static class TelegramMessageHelper
    {
        private const string MESSAGE_LOG_EXECUTE = "<b>Execução do aplicativo de call solana</b>\r\n" +
                                                   "<b>Data Execução: </b>{0}.\r\n" +
                                                   "<i><b>Proxima execução</b> no período timer de --> {1}</i>";
        private const string MESSAGE_LOG_APP_RUNNING = "<b>Aplicativo está rodando.</b>\r\n" +
                                                       "<i><b>Não irá efetuar essa execução:</b> {0}</i>.\r\n" +
                                                       "<i><b>Proxima execução</b> no período timer de --> {1}</i>";
        private const string MESSAGE_LOG_EXECUTE_ERROR = "<b>Aplicativo suspendeu a execução.</b>\r\n" +
                                                         "<i><b>Mensagem de erro:</b> {0}</i>.\r\n" +
                                                         "StackTrace: {1}\r\n" +
                                                         "<i><b>Proxima execução</b> no período timer de --> {2}. \r\n" +
                                                         "<b>Dev's Favor verificar</b> Cc:@morpheus.gmd , @euRodrigo</i>";
        private const string MESSAGE_BUY_MESSAGE = "<b>*** NEW BUY ALERT ***</b>\r\n" +
                                                   "<tg-emoji emoji-id='5368324170671202286'>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>\r\n" +
                                                   "<i>WalletHash:</i> {0}\r\n" +
                                                   "<i>ClassWallet:</i> {1} \r\n" +
                                                   "<i>Token:</i> {2}\r\n" +
                                                   "<i>Ca:</i> {3}\r\n" +
                                                   "<i>Minth Authority:</i> {4}\r\n" +
                                                   "<i>Freeze Authority:</i> {5}\r\n" +
                                                   "<i>Is Mutable:</i> {6}\r\n" +
                                                   "<i>Quantity:</i> {7}\r\n" +
                                                   "<i>Price:</i> {8}\r\n" +
                                                   "<i>Date:</i> {9}";
        private const string MESSAGE_REBUY_MESSAGE = "<b>*** NEW REBUY ALERT ***</b>\r\n" +
                                                     "<tg-emoji emoji-id='5368324170671202286'>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>\r\n" +
                                                     "<i>WalletHash:</i> {0}\r\n" +
                                                     "<i>ClassWallet:</i> {1} \r\n" +
                                                     "<i>Token:</i> {2}\r\n" +
                                                     "<i>Ca:</i> {3}\r\n" +
                                                     "<i>Minth Authority:</i> {4}\r\n" +
                                                     "<i>Freeze Authority:</i> {5}\r\n" +
                                                     "<i>Is Mutable:</i> {6}\r\n" +
                                                     "<i>Quantity:</i> {7}\r\n" +
                                                     "<i>Price:</i> {8}\r\n" +
                                                     "<i>Date:</i> {9}";
        private const string MESSAGE_SELL_MESSAGE = "<b>*** NEW SELL ALERT ***</b>\r\n" +
                                                    "<tg-emoji emoji-id='5368324170671202286'>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>\r\n" +
                                                    "<i>WalletHash:</i> {0}\r\n" +
                                                    "<i>ClassWallet:</i> {1} \r\n" +
                                                    "<i>Token:</i> {2}\r\n" +
                                                    "<i>Quantity:</i> {3}\r\n" +
                                                    "<i>Price:</i> {4}\r\n" +
                                                    "<i>Date:</i> {5}";
        private const string MESSAGE_MM_NEW_BUY_MESSAGE = "<b>*** NEW MM BUY ALERT ***</b>\r\n" +
                                                           "<tg-emoji emoji-id='5368324170671202286'>🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢🟢</tg-emoji>\r\n" +
                                                           "<i>WalletHash:</i> {0}\r\n" +
                                                           "<i>ClassWallet:</i> {1} \r\n" +
                                                           "<i>Token:</i> {2}\r\n" +
                                                           "<i>Ca:</i> {3}\r\n" +
                                                           "<i>Minth Authority:</i> {4}\r\n" +
                                                           "<i>Freeze Authority:</i> {5}\r\n" +
                                                           "<i>Is Mutable:</i> {6}\r\n" +
                                                           "<i>Quantity:</i> {7}\r\n" +
                                                           "<i>Price:</i> {8}\r\n" +
                                                           "<i>Date:</i> {9}\r\n" +
                                                           "Cc: @all";
        private const string MESSAGE_MM_REBUY_MESSAGE = "<b>*** NEW MM REBUY ALERT ***</b>\r\n" +
                                                        "<tg-emoji emoji-id='5368324170671202286'>🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵🔵</tg-emoji>\r\n" +
                                                        "<i>WalletHash:</i> {0}\r\n" +
                                                        "<i>ClassWallet:</i> {1} \r\n" +
                                                        "<i>Token:</i> {2}\r\n" +
                                                        "<i>Ca:</i> {3}\r\n" +
                                                        "<i>Minth Authority:</i> {4}\r\n" +
                                                        "<i>Freeze Authority:</i> {5}\r\n" +
                                                        "<i>Is Mutable:</i> {6}\r\n" +
                                                        "<i>Quantity:</i> {7}\r\n" +
                                                        "<i>Price:</i> {8}\r\n" +
                                                        "<i>Date:</i> {9}";
        private const string MESSAGE_MM_SELL_MESSAGE = "<b>*** NEW MM SELL ALERT ***</b>\r\n" +
                                                       "<tg-emoji emoji-id='5368324170671202286'>🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴🔴</tg-emoji>\r\n" +
                                                       "<i>WalletHash:</i> {0}\r\n" +
                                                       "<i>ClassWallet:</i> {1} \r\n" +
                                                       "<i>Token:</i> {2}\r\n" +
                                                       "<i>Quantity:</i> {3}\r\n" +
                                                       "<i>Price:</i> {4}\r\n" +
                                                       "<i>Date:</i> {5}\r\n" +
                                                       "Cc @all";
        public static string GetFormatedMessage(ETypeMessage eTypeMessage, object[] args)
        {
            switch (eTypeMessage)
            {
                case ETypeMessage.LOG_EXECUTE:
                    return string.Format(MESSAGE_LOG_EXECUTE, args);
                case ETypeMessage.LOG_APP_RUNNING:
                    return string.Format(MESSAGE_LOG_APP_RUNNING, args);
                case ETypeMessage.LOG_EXECUTE_ERROR:
                    return string.Format(MESSAGE_LOG_EXECUTE_ERROR, args);
                case ETypeMessage.BUY_MESSAGE:
                    return string.Format(MESSAGE_BUY_MESSAGE, args);
                case ETypeMessage.REBUY_MESSAGE:
                    return string.Format(MESSAGE_REBUY_MESSAGE, args);
                case ETypeMessage.SELL_MESSAGE:
                    return string.Format(MESSAGE_SELL_MESSAGE, args);
                case ETypeMessage.MM_NEW_BUY_MESSAGE:
                    return string.Format(MESSAGE_MM_NEW_BUY_MESSAGE, args);
                case ETypeMessage.MM_REBUY_MESSAGE:
                    return string.Format(MESSAGE_MM_REBUY_MESSAGE, args);
                case ETypeMessage.MM_SELL_MESSAGE:
                    return string.Format(MESSAGE_MM_SELL_MESSAGE, args);
                default:
                    return string.Empty;
            }
        }
    }

    public enum ETypeMessage 
    { 
        LOG_EXECUTE = 1,
        LOG_APP_RUNNING,
        LOG_EXECUTE_ERROR,
        BUY_MESSAGE,
        REBUY_MESSAGE,
        SELL_MESSAGE,
        MM_NEW_BUY_MESSAGE,
        MM_REBUY_MESSAGE,
        MM_SELL_MESSAGE
    }
}
