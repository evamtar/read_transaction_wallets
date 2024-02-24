using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Service.InternalServices.Domains
{
    public class ClassWalletService : CachedServiceBase<ClassWallet>, IClassWalletService
    {
        public ClassWalletService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
