using Entity = SyncronizationBot.Domain.Model.Database;
using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;

namespace SyncronizationBot.Service.InternalServices.Token
{
    public class TokenService : CachedServiceBase<Entity.Token>, ITokenService
    {
        public TokenService(ITokenRepository repository, CACHE.ITokenMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
