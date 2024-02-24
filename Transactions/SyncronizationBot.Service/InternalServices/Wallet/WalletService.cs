using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Repository.MongoDB;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletService : CachedServiceBase<Entity.Wallet>, IWalletService
    {
        public WalletService(IWalletRepository repository, IWalletMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
