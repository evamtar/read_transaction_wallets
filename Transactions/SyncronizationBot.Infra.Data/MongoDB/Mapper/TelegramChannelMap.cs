using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TelegramChannelMap : BaseMapper<TelegramChannel>
    {
        public TelegramChannelMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TelegramChannel> builder)
        {
            builder.Ignore(tc => tc.AlertPrices);
            builder.Ignore(tc => tc.AlertsConfigurations);
            builder.Ignore(tc => tc.TelegramMessages);
        }
    }
}
