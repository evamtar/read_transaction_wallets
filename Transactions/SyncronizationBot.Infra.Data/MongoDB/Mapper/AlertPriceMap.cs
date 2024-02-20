using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class AlertPriceMap : BaseMapper<AlertPrice>
    {
        public AlertPriceMap() : base(EDatabase.Mongodb)
        {
        }
        protected override void PropertiesWithConversion(EntityTypeBuilder<AlertPrice> builder)
        {
            //builder.Property(ap => ap.PriceBase).HasConversion<string?>();
            //builder.Property(ap => ap.PriceValue).HasConversion<string?>();
            //builder.Property(ap => ap.PricePercent).HasPrecision(5, 2);
        }

        protected override void RelationsShips(EntityTypeBuilder<AlertPrice> builder)
        {
            builder.Ignore(ap => ap.TelegramChannel);
        }
    }
}
