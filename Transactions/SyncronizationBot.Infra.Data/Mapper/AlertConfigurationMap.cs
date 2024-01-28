using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class AlertConfigurationMap : IEntityTypeConfiguration<AlertConfiguration>
    {
        public void Configure(EntityTypeBuilder<AlertConfiguration> builder)
        {
            builder.ToTable("AlertConfiguration");
            builder.Property(ac => ac.ID);
            builder.Property(ac => ac.Name);
            builder.Property(ac => ac.TypeAlert);
            builder.Property(ac => ac.TelegramChannelId);
            builder.Property(ac => ac.IsActive);
            builder.Property(ac => ac.CreateDate);
            builder.Property(ac => ac.LastUpdate);
            builder.HasOne(ac => ac.TelegramChannel).WithMany(tc => tc.AlertsConfigurations).HasForeignKey(ac => ac.TelegramChannelId);
            builder.HasKey(ac => ac.ID);
        }
    }
}
