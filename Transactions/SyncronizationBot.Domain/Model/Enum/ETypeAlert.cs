

namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETypeAlert
    {
        LOG_EXECUTE = -1,
        LOG_APP_RUNNING = -2,
        LOG_ERROR = -3,
        LOG_LOST_CONFIGURATION = -4,
        NONE = 0,
        BUY = 1,
        REBUY = 2,
        SELL = 3,
        SWAP = 4,
        POOL_CREATE = 5,
        POOL_FINISH = 6,
        ALERT_PRICE = 7,
        ALERT_TOKEN_ALPHA = 8,
        ALERT_WHALE_TRANSACTION = 9,
        ALERT_INFLUENCER_TRANSACTION = 10,
    }
}
