using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;



namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class AlertParameterRepository : MongoRepository<AlertParameter>, IAlertParameterRepository
    {
        public AlertParameterRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
