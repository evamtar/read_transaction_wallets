using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.MongoDB;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository.Base;


namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TelegramMessageRepository : CachedRepository<TelegramMessage>, ITelegramMessageRepository
    {
        public TelegramMessageRepository(MongoDbContext context) : base(context)
        {

        }
    }
}
