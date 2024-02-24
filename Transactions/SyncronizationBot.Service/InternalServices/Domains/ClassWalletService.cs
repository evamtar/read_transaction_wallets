using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Service.InternalServices.Base;

namespace SyncronizationBot.Service.InternalServices.Domains
{
    public class ClassWalletService : CachedServiceBase<ClassWallet>, IClassWalletService
    {
        public ClassWalletService(IClassWalletRepository repository, CACHE.IClassWalletMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
