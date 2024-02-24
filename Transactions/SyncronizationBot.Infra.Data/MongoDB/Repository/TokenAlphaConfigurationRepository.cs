using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;


namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TokenAlphaConfigurationRepository : MongoRepository<TokenAlphaConfiguration>, ITokenAlphaConfigurationRepository
    {
        public TokenAlphaConfigurationRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
