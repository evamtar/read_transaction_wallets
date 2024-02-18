using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TransactionsMap : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.ToTable("Transactions");
            builder.Property(t => t.ID);
            builder.Property(t => t.Signature);
            builder.Property(t => t.DateTransactionUTC);
            builder.Property(t => t.FeeTransaction).HasConversion<string?>();
            builder.Property(t => t.PriceSol).HasConversion<string?>();
            builder.Property(t => t.TotalOperationSol).HasConversion<string?>();
            builder.Property(t => t.WalletId);
            builder.Property(t => t.TypeOperationId);
            builder.Ignore(t => t.WalletHash);
            builder.Ignore(t => t.ClassWallet);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.WalletId);
            builder.HasOne(t => t.TypeOperation).WithMany(w => w.Transactions).HasForeignKey(t => t.TypeOperationId);
            builder.HasKey(t => t.ID);
        }
    }
}
