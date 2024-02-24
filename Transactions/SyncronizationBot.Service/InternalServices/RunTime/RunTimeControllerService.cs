using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;

namespace SyncronizationBot.Service.InternalServices.RunTime
{
    public class RunTimeControllerService : CachedServiceBase<RunTimeController>, IRunTimeControllerService
    {
        public RunTimeControllerService(IRunTimeControllerRepository repository, IRunTimeControllerMongoDBRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
