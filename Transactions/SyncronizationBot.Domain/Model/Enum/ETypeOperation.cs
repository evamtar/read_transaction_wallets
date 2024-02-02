

namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETypeOperation
    {
        NONE = 0,
        BUY = 1,
        SELL = 2,
        SEND = 3,
        RECEIVED = 4,
        SWAP = 5,
        POOLCREATE,
        POOLFINALIZED,
        BURN
    }
}
