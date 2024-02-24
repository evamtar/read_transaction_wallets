using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.RecoveryService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Repository.MongoDB;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceService : CachedServiceBase<Entity.WalletBalance>, IWalletBalanceService
    {
        public WalletBalanceService(IWalletBalanceRepository repository, IWalletBalanceMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
