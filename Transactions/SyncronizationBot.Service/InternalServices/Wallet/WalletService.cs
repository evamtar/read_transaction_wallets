using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletService : CachedServiceBase<Entity.Wallet>, IWalletService
    {
        public WalletService(IWalletRepository repository, CACHE.IWalletRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
