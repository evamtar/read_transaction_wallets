using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TransactionNotMappedMap : IEntityTypeConfiguration<TransactionNotMapped>
    {
        public void Configure(EntityTypeBuilder<TransactionNotMapped> builder)
        {
            builder.ToTable("TransactionNotMapped");
            builder.Property(t => t.ID);
            builder.Property(t => t.IdWallet); 
            builder.Property(t => t.Signature);
            builder.Property(t => t.Link);
            builder.Property(t => t.Error);
            builder.Property(t => t.StackTrace);
            builder.Property(t => t.DateTimeRunner);
            builder.HasKey(t => t.ID);
        }
    }
}
