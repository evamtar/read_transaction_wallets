using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;



namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class RunTimeControllerMap : BaseMapper<RunTimeController>
    {
        public RunTimeControllerMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void AddKeys(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.Ignore(rt => rt.ID);
            builder.Ignore(rt => rt.CachedId);
            builder.HasKey(rt => rt.RuntimeId);
        }
        
        protected override void PropertiesWithConversion(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.Property(rt => rt.ConfigurationTimer).HasConversion<string>();
        }

    }
}
