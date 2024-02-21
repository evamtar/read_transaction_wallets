using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class AlertPriceMap : BaseMapper<AlertPrice>
    {
        public AlertPriceMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<AlertPrice> builder)
        {
            builder.HasOne(ap => ap.TelegramChannel).WithMany(tc => tc.AlertPrices).HasForeignKey(ap => ap.TelegramChannelId);
        }
    }
}
