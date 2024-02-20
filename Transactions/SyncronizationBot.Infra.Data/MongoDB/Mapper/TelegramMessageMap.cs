using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TelegramMessageMap : BaseMapper<TelegramMessage>
    {
        public TelegramMessageMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TelegramMessage> builder)
        {
            builder.Ignore(tm => tm.TelegramChannel);
        }
    }
}
