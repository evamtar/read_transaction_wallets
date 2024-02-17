using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Service.InternalServices.Token
{
    public class TokenService : ServiceBase<Entity.Token>, ITokenService
    {
        public TokenService(ITokenRepository repository) : base(repository)
        {
        }
    }
}
