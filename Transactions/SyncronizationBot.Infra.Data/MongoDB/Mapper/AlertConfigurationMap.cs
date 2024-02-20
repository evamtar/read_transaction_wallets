using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class AlertConfigurationMap : BaseMapper<AlertConfiguration>
    {
        public AlertConfigurationMap() : base(EDatabase.Mongodb)
        {
            
        }

        protected override void IgnoreRelationsShips(EntityTypeBuilder<AlertConfiguration> builder)
        {
            builder.Ignore(ac => ac.TelegramChannel);
            builder.Ignore(ac => ac.TypeOperation);
        }
    }
}
