using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Base;

namespace SyncronizationBot.Domain.Service.InternalService.Token
{
    public interface ITokenService : ICachedServiceBase<Entity.Token>
    {
    }
}
