using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class AlertConfigurationMap : BaseMapper<AlertConfiguration>
    {
        public AlertConfigurationMap() : base(EDatabase.SqlServer) 
        { 
        }
        
        protected override void RelationsShips(EntityTypeBuilder<AlertConfiguration> builder)
        {
            builder.HasOne(ac => ac.TelegramChannel).WithMany(tc => tc.AlertsConfigurations).HasForeignKey(ac => ac.TelegramChannelId);
            builder.HasOne(ac => ac.TypeOperation).WithMany(tc => tc.AlertConfigurations).HasForeignKey(ac => ac.TypeOperationId);
        }
    }
}
