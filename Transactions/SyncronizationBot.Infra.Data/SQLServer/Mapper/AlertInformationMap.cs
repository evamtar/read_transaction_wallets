using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class AlertInformationMap : BaseMapper<AlertInformation>
    {
        public AlertInformationMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<AlertInformation> builder)
        {
            builder.HasOne(ai => ai.AlertConfiguration).WithMany(ac => ac.AlertsInformations).HasForeignKey(ai => ai.AlertConfigurationId);
        }
        
    }
}
