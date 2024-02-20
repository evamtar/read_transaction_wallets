using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class AlertParameterMap : BaseMapper<AlertParameter>
    {
        public AlertParameterMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<AlertParameter> builder)
        {
            builder.HasOne(ap => ap.AlertInformation).WithMany(ai => ai.AlertsParameters).HasForeignKey(ap => ap.AlertInformationId);
        }
    }
}
