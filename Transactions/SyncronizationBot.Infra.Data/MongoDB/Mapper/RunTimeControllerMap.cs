using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.Database;



namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class RunTimeControllerMap : IEntityTypeConfiguration<RunTimeController>
    {
        public void Configure(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.ToCollection(typeof(RunTimeController).Name);
            builder.Property(rt => rt.RuntimeId);
            builder.Property(rt => rt.ConfigurationTimer).HasConversion<string>();
            builder.Property(rt => rt.TypeService);
            builder.Property(rt => rt.IsRunning);
            builder.Property(rt => rt.IsContingecyTransaction);
            builder.Property(rt => rt.TimesWithoutTransaction);
            builder.Property(rt => rt.JobName);
            builder.Property(rt => rt.JobDescription);
            builder.Property(rt => rt.IsActive);
            builder.Property(rt => rt.RuntimeParentId);
            builder.Ignore(rt => rt.Parent);
            builder.Ignore(rt => rt.ID);
            builder.HasKey(rt => rt.RuntimeId);
        }
    }
}
