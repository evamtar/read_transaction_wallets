using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenPriceHistoryRepository : CachedRepository<TokenPriceHistory>, ITokenPriceHistoryRepository
    {
        public TokenPriceHistoryRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
