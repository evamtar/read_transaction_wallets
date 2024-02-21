using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class AlertParameterMap : BaseMapper<AlertParameter>
    {
        public AlertParameterMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<AlertParameter> builder)
        {
            builder.Ignore(ap => ap.AlertInformation);
        }
        
    }
}
