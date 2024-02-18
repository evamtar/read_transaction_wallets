using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper.SqlServer
{
    public class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallet");
            builder.Property(w => w.ID);
            builder.Property(w => w.Hash);
            builder.Property(w => w.ClassWalletId);
            builder.Property(w => w.IsLoadBalance);
            builder.Property(w => w.DateLoadBalance);
            builder.Property(w => w.IsActive);
            builder.Property(w => w.LastUpdate);
            builder.HasOne(w => w.ClassWallet).WithMany(cw => cw.Wallets).HasForeignKey(w => w.ClassWalletId);
            builder.HasKey(w => w.ID);
        }
    }
}
