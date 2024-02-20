using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class AlertInformationMap : BaseMapper<AlertInformation>
    {
        public AlertInformationMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreRelationsShips(EntityTypeBuilder<AlertInformation> builder)
        {
            builder.Ignore(ai => ai.AlertConfiguration);
        }
    }
}
