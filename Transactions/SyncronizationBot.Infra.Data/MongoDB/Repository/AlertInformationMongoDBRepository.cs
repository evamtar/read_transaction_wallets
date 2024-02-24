using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class AlertInformationMongoDBRepository : MongoRepository<AlertInformation>, IAlertInformationMongoDBRepository
    {
        public AlertInformationMongoDBRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
