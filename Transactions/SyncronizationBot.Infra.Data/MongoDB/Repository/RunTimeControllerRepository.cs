using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class RunTimeControllerRepository : CachedRepository<RunTimeController>, IRunTimeControllerRepository
    {
        public RunTimeControllerRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
