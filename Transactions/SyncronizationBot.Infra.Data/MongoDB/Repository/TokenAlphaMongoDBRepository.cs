using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;



namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenAlphaMongoDBRepository : MongoRepository<TokenAlpha>, ITokenAlphaMongoDBRepository
    {
        public TokenAlphaMongoDBRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
