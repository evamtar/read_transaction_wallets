using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Service.InternalServices.RunTime
{
    public class RunTimeControllerService : CachedServiceBase<RunTimeController>, IRunTimeControllerService
    {
        public RunTimeControllerService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
