
namespace SyncronizationBot.Domain.Model.Enum
{
    public enum ETypeService
    {
        NONE = 0,
        TransactionService = 1,
        BalanceInsert = 2,
        BalanceUpdate = 3,
        PriceAlert = 4,
        DeleteFromChannels = 5,
        TransactionForHistory = 6,
        NewTokensFromRaydium = 7 //Futuro com API da Raydium
    }
}
