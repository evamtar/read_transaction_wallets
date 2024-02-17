using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.RecoveryService.Base;

namespace SyncronizationBot.Domain.Service.InternalService.Token
{
    public interface ITokenService : IServiceBase<Entity.Token>
    {
    }
}
