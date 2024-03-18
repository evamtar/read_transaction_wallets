using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TransactionsRPCRecoveryMap : IEntityTypeConfiguration<TransactionsRPCRecovery>
    {
        public void Configure(EntityTypeBuilder<TransactionsRPCRecovery> builder)
        {
            builder.ToTable("TransactionsRPCRecovery");
            builder.Property(tc => tc.ID);
            builder.Property(tc => tc.Signature);
            builder.Property(tc => tc.DateOfTransaction);
            builder.Property(tc => tc.BlockTime).HasConversion<string?>();
            builder.Property(tc => tc.WalletId);
            builder.Property(tc => tc.CreateDate);
            builder.Property(tc => tc.IsIntegrated);
            builder.Property(tc => tc.IsDCA);
            builder.HasOne(tc => tc.Wallet).WithMany(w => w.TransactionsContingencies).HasForeignKey(tc => tc.WalletId);
            builder.HasKey(tc => tc.ID);
        }
    }
}
