

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class PublishMessageMap : IEntityTypeConfiguration<PublishMessage>
    {
        public void Configure(EntityTypeBuilder<PublishMessage> builder)
        {
            builder.ToTable("PublishMessage");
            builder.Property(pm => pm.ID);
            builder.Property(pm => pm.EntityId);
            builder.Property(pm => pm.Entity);
            builder.Property(pm => pm.JsonValue);
            builder.Property(pm => pm.ItWasPublished);
            builder.Property(pm => pm.EntityParentId);
            builder.HasOne(pm => pm.EntityParent).WithMany(pmc => pmc.EntityChildrens).HasForeignKey(pm => pm.EntityParentId).IsRequired(false);
            builder.HasKey(pm => pm.ID);
        }
    }
}
