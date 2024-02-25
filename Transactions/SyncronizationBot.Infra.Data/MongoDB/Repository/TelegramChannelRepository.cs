using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.MongoDB.Context;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class TelegramChannelRepository : MongoRepository<TelegramChannel>, ITelegramChannelRepository
    {
        public TelegramChannelRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
