using CACHE = SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Service.InternalServices.RunTime
{
    public class RunTimeControllerService : CachedServiceBase<RunTimeController>, IRunTimeControllerService
    {
        public RunTimeControllerService(IRunTimeControllerRepository repository, CACHE.IRunTimeControllerRepository cachedRepository) : base(repository, cachedRepository)
        {
        }
    }
}
