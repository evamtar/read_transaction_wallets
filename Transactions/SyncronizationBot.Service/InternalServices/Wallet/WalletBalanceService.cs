using Entity = SyncronizationBot.Domain.Model.Database;
using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Domain.Service.RecoveryService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceService : CachedServiceBase<Entity.WalletBalance>, IWalletBalanceService
    {
        public WalletBalanceService(IWalletBalanceRepository repository, CACHE.IWalletBalanceMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
