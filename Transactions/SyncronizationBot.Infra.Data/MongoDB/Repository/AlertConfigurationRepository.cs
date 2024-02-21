using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;
using SyncronizationBot.Infra.Data.MongoDB.Context;


namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class AlertConfigurationRepository : CachedRepository<AlertConfiguration>, IAlertConfigurationRepository
    {
        public AlertConfigurationRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
