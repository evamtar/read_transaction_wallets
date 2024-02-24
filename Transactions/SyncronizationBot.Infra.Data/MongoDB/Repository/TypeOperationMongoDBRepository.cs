using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    internal class TypeOperationMongoDBRepository : MongoRepository<TypeOperation>, ITypeOperationMongoDBRepository
    {
        public TypeOperationMongoDBRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
