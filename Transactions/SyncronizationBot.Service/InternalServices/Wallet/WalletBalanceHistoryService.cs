using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Domain.Repository.MongoDB;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceHistoryService : CachedServiceBase<Entity.WalletBalanceHistory>, IWalletBalanceHistoryService
    {
        public WalletBalanceHistoryService(IWalletBalanceHistoryRepository readRepository, IWalletBalanceHistoryMongoDBRepository cachedRepository) : base(readRepository, cachedRepository)
        {
        }
    }
}
