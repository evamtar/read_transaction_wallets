

namespace SyncronizationBot.Utils
{
    public static class Constants
    {
        /* COMMOM INSTRUCTIONS */
        public const string INSTRUCTION_INSERT = "INSERT";
        public const string INSTRUCTION_UPDATE = "UPDATE";
        public const string INSTRUCTION_DELETE = "DELETE";
        
        /* SPECIAL INSTRUCTION */
        public const string INSTRUCTION_UPDATE_RANGE_WB = "UPDATE_RANGE_WALLET_BALANCE";

        public const string ALERT_PRICE_INSTRUCTION = "ALERTPRICE";
        public const string RUN_TIME_CONTROLLER_INSTRUCTION = "RUNTIMECONTROLLER";
        public const string TELEGRAM_MESSAGE_INSTRUCTION = "TELEGRAMMESSAGE";
        public const string TOKEN_INSTRUCTION = "TOKEN";
        public const string TOKEN_PRICE_HISTORY_INSTRUCTION = "TOKENPRICEHISTORY";
        public const string TOKEN_SECURITY_INSTRUCTION = "TOKENSECURITY";
        public const string WALLET_INSTRUCTION = "WALLET";
        public const string WALLET_BALANCE_INSTRUCTION = "WALLETBALANCE";
        public const string WALLET_BALANCE_HISTORY_INSTRUCTION = "WALLETBALANCEHISTORY";
        
        

        public const string LOG_EXECUTE = "LOGEXECUTE";
        public const string LOG_ERROR = "LOGERROR";
        public const string LOG_APP_RUNNING = "LOGAPPRUNNING";
        public const string LOG_LOST_CONFIGURATIO = "LOGLOSTCONFIGURATION";
    }
}
