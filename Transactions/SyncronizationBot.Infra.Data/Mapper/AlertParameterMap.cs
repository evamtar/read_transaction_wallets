using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class AlertParameterMap : IEntityTypeConfiguration<AlertParameter>
    {
        public void Configure(EntityTypeBuilder<AlertParameter> builder)
        {
            builder.ToTable("AlertParameter");
            builder.Property(ap => ap.ID);
            builder.Property(ap => ap.Name);
            builder.Property(ap => ap.AlertInformationId);
            builder.Property(ap => ap.Class);
            builder.Property(ap => ap.Parameter);
            builder.Property(ap => ap.FixValue);
            builder.Property(ap => ap.IsIcon);
            builder.Property(ap => ap.IsImage);
            builder.HasOne(ap => ap.AlertInformation).WithMany(ai => ai.AlertsParameters).HasForeignKey(ap => ap.AlertInformationId);
            builder.HasKey(ai => ai.ID);
        }
    }
}
