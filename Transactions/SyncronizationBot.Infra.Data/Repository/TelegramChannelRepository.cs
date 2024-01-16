using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Infra.Data.Context;
using SyncronizationBot.Infra.Data.Repository.Base;


namespace SyncronizationBot.Infra.Data.Repository
{
    public class TelegramChannelRepository : Repository<TelegramChannel>, ITelegramChannelRepository
    {
        public TelegramChannelRepository(SqlContext context) : base(context)
        {

        }
    }
}
