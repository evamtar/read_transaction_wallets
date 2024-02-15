using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TypeOperationMap : IEntityTypeConfiguration<TypeOperation>
    {
        public void Configure(EntityTypeBuilder<TypeOperation> builder)
        {
            builder.ToTable("TypeOperation");
            builder.Property(to => to.ID);
            builder.Property(to => to.Name);
            builder.Property(to => to.IdTypeOperation);
            builder.HasKey(to => to.ID);
        }
    }
}
