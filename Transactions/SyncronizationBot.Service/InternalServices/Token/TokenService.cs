using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Token;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Service.InternalServices.Token
{
    public class TokenService : CachedServiceBase<Entity.Token>, ITokenService
    {
        public TokenService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
