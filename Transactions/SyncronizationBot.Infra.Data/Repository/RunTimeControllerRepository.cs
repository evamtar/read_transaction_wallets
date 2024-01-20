using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Infra.Data.Context;
using SyncronizationBot.Infra.Data.Repository.Base;


namespace SyncronizationBot.Infra.Data.Repository
{
    public class RunTimeControllerRepository : Repository<RunTimeController>, IRunTimeControllerRepository
    {
        public RunTimeControllerRepository(SqlContext context) : base(context)
        {

        }
    }
}
