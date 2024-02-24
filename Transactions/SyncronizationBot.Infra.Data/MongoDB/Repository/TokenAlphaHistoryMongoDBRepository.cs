using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;



namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenAlphaHistoryMongoDBRepository : MongoRepository<TokenAlphaHistory>, ITokenAlphaHistoryMongoDBRepository
    {
        public TokenAlphaHistoryMongoDBRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
