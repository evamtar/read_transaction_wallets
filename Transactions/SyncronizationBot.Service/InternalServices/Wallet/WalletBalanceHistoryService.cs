using Entity = SyncronizationBot.Domain.Model.Database;
using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.Wallet;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceHistoryService : CachedServiceBase<Entity.WalletBalanceHistory>, IWalletBalanceHistoryService
    {
        public WalletBalanceHistoryService(IWalletBalanceHistoryRepository readRepository, CACHE.IWalletBalanceHistoryMongoDBRepository cachedRepository) : base(readRepository, cachedRepository)
        {
        }
    }
}
