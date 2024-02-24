using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TelegramChannelReadyOnlyRepository : SqlServerReadyOnlyRepository<TelegramChannel>, ITelegramChannelReadyOnlyRepository
    {
        public TelegramChannelReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
