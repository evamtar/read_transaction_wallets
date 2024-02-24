using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TransactionsMongoDBRepository : MongoRepository<Transactions>, ITransactionsMongoDBRepository
    {
        public TransactionsMongoDBRepository(MongoDbContext context) : base(context)
        {

        }

    }
}
