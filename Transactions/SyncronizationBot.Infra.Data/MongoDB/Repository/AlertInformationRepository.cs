using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.MongoDB.Context;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class AlertInformationRepository : MongoRepository<AlertInformation>, IAlertInformationRepository
    {
        public AlertInformationRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
