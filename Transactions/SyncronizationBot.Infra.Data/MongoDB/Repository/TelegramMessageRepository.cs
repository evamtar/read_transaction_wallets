using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.MongoDB.Context;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TelegramMessageRepository : MongoRepository<TelegramMessage>, ITelegramMessageRepository
    {
        public TelegramMessageRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
