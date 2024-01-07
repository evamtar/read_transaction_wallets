
namespace ReadTransactionsWallets.Utils
{
    public static class TelegramMessageHelper
    {
        private const string MESSAGE_LOG_EXECUTE = "<b>Execução do aplicativo de call solana</b>\r\n<b>Data Execução: </b>{0}.\r\n<i><b>Proxima execução</b> no período timer de --> {1}</i>";
        private const string MENSAGEM_LOG_APP_RUNNING = "<b>Aplicativo está rodando.</b>\r\n<i><b>Não irá efetuar essa execução:</b> {0}</i>.\r\n<i><b>Proxima execução</b> no período timer de --> {1}</i>";
        private const string MENSAGEM_LOG_EXECUTE_ERROR = "<b>Aplicativo suspendeu a execução.</b>\r\n<i><b>Mensagem de erro:</b> {0}</i>.\r\nStackTrace: {1}\r\n<i><b>Proxima execução</b> no período timer de --> {2}. \r\n<b>Dev's Favor verificar</b> Cc:@morpheus.gmd</i>";
        public static string GetFormatedMessage(ETypeMessage eTypeMessage, object[] args)
        {
            switch (eTypeMessage)
            {
                case ETypeMessage.LOG_EXECUTE:
                    return string.Format(MESSAGE_LOG_EXECUTE, args);
                case ETypeMessage.LOG_APP_RUNNING:
                    return string.Format(MENSAGEM_LOG_APP_RUNNING, args);
                case ETypeMessage.LOG_EXECUTE_ERROR:
                    return string.Format(MENSAGEM_LOG_EXECUTE_ERROR, args);
                case ETypeMessage.BUY_MESSAGE:
                    return string.Format("", args);
                case ETypeMessage.REBUY_MESSAGE:
                    return string.Format("", args);
                case ETypeMessage.SELL_MESSAGE:
                    return string.Format("", args);
                case ETypeMessage.NEM_MM_BUY_MESSAGE:
                    return string.Format("", args);
                case ETypeMessage.REBUY_MM_BUY_MESSAGE:
                    return string.Format("", args);
                case ETypeMessage.SELL_MM_MESSAGE:
                    return string.Format("", args);
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
        NEM_MM_BUY_MESSAGE,
        REBUY_MM_BUY_MESSAGE,
        SELL_MM_MESSAGE
    }
}
