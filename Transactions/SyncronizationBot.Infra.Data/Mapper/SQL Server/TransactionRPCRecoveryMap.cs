using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper.SqlServer
{
    public class TransactionRPCRecoveryMap : IEntityTypeConfiguration<TransactionRPCRecovery>
    {
        public void Configure(EntityTypeBuilder<TransactionRPCRecovery> builder)
        {
            builder.ToTable("TransactionRPCRecovery");
            builder.Property(tc => tc.ID);
            builder.Property(tc => tc.Signature);
            builder.Property(tc => tc.DateOfTransaction);
            builder.Property(tc => tc.BlockTime).HasConversion<string?>();
            builder.Property(tc => tc.WalletId);
            builder.Property(tc => tc.CreateDate);
            builder.Property(tc => tc.IsIntegrated);
            builder.HasOne(tc => tc.Wallet).WithMany(w => w.TransactionsRPCRecovery).HasForeignKey(tc => tc.WalletId);
            builder.HasKey(tc => tc.ID);
        }
    }
}
