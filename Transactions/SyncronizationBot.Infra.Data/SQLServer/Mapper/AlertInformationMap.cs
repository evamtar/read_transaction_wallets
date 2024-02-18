using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class AlertInformationMap : IEntityTypeConfiguration<AlertInformation>
    {
        public void Configure(EntityTypeBuilder<AlertInformation> builder)
        {
            builder.ToTable("AlertInformation");
            builder.Property(ai => ai.ID);
            builder.Property(ai => ai.Message);
            builder.Property(ai => ai.IdClassification);
            builder.Property(ai => ai.AlertConfigurationId);
            builder.HasOne(ai => ai.AlertConfiguration).WithMany(ac => ac.AlertsInformations).HasForeignKey(ai => ai.AlertConfigurationId);
            builder.HasKey(ai => ai.ID);
        }
    }
}
