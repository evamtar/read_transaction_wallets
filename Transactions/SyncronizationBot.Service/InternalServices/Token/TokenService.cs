using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Repository.MongoDB;

namespace SyncronizationBot.Service.InternalServices.Token
{
    public class TokenService : CachedServiceBase<Entity.Token>, ITokenService
    {
        public TokenService(ITokenRepository repository, ITokenMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
