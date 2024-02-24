using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;
using SyncronizationBot.Infra.Data.MongoDB.Context;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenAlphaWalletMongoDBRepository : MongoRepository<TokenAlphaWallet>, ITokenAlphaWalletMongoDBRepository
    {
        public TokenAlphaWalletMongoDBRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
