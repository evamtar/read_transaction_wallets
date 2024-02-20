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

        protected override void PropertiesWithConversion(EntityTypeBuilder<TelegramChannel> builder)
        {
            //builder.Property(cw => cw.ChannelId).HasPrecision(30, 0);
        }
    }
}
