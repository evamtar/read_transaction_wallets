using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenAlphaWalletHistoryMongoDBRepository : MongoRepository<TokenAlphaWalletHistory>, ITokenAlphaWalletHistoryMongoDBRepository
    {
        public TokenAlphaWalletHistoryMongoDBRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
