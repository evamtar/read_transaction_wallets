using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Repository.Base
{
    public class AlertParameterMap : BaseMapper<AlertParameter>
    {
        public AlertParameterMap() : base(EDatabase.Mongodb)
        {
        }
        protected override void IgnoreRelationsShips(EntityTypeBuilder<AlertParameter> builder)
        {
            builder.Ignore(ap => ap.AlertInformation);
        }
    }

}
