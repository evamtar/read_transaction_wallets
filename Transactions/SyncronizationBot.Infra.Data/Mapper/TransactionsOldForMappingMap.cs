using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TransactionsOldForMappingMap : IEntityTypeConfiguration<TransactionsOldForMapping>
    {
        public void Configure(EntityTypeBuilder<TransactionsOldForMapping> builder)
        {
            builder.ToTable("TransactionsOldForMapping");
            builder.Property(tofm => tofm.ID);
            builder.Property(tofm => tofm.Signature);
            builder.Property(tofm => tofm.DateOfTransaction);
            builder.Property(tofm => tofm.WalletId);
            builder.Property(tofm => tofm.CreateDate);
            builder.Property(tofm => tofm.IsIntegrated);
            builder.HasOne(tofm => tofm.Wallet).WithMany(w => w.TransactionsOldForMapping).HasForeignKey(tofm => tofm.WalletId);
            builder.HasKey(tofm => tofm.ID);
        }
    }
}
