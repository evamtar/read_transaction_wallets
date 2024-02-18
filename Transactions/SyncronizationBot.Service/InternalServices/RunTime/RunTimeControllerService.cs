using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Service.InternalServices.Base;

namespace SyncronizationBot.Service.InternalServices.RunTime
{
    public class RunTimeControllerService : ServiceBase<RunTimeController>, IRunTimeControllerService
    {
        public RunTimeControllerService(IRunTimeControllerRepository repository) : base(repository)
        {
        }
    }
}
