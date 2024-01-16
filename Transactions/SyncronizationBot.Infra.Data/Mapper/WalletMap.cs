using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallet");
            builder.Property(w => w.ID);
            builder.Property(w => w.Hash);
            builder.Property(w => w.IdClassWallet);
            builder.HasOne(w => w.ClassWallet).WithMany(cw => cw.Wallets).HasForeignKey(w => w.IdClassWallet);
            builder.HasKey(w => w.ID);
        }
    }
}
