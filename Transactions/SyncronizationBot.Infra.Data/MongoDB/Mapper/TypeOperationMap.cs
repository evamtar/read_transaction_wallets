using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TypeOperationMap : IEntityTypeConfiguration<TypeOperation>
    {
        public void Configure(EntityTypeBuilder<TypeOperation> builder)
        {
            builder.ToCollection(typeof(TypeOperation).Name);
            builder.Property(to => to.ID);
            builder.Property(to => to.Name);
            builder.Property(to => to.IdTypeOperation);
            builder.HasKey(to => to.ID);
        }
    }
}
