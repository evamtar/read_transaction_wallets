using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;



namespace SyncronizationBot.Infra.Data.Mapper
{
    public class RunTimeControllerMap : IEntityTypeConfiguration<RunTimeController>
    {
        public void Configure(EntityTypeBuilder<RunTimeController> builder)
        {
            builder.ToTable("RunTimeController");
            builder.Property(rt => rt.IdRuntime);
            builder.Property(rt => rt.ConfigurationTimer);
            builder.Property(rt => rt.TypeService); 
            builder.Property(rt => rt.IsRunning);
            builder.Property(rt => rt.IsContingecyTransactions);
            builder.Property(rt => rt.TimesWithoutTransactions); 
            builder.Ignore(rt => rt.ID);
            builder.HasKey(rt => rt.IdRuntime);
        }
    }
}
