using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadTransactionsWallets.Domain.Model.Database;

namespace ReadTransactionsWallets.Infra.Data.Mapper
{
    public class WalletBalanceMap : IEntityTypeConfiguration<WalletBalance>
    {
        public void Configure(EntityTypeBuilder<WalletBalance> builder)
        {
            builder.ToTable("ClassWallet");
            builder.Property(wb => wb.ID);
            builder.Property(wb => wb.DateUpdate);
            builder.Property(wb => wb.IdWallet);
            builder.Property(wb => wb.IdToken);
            builder.Property(wb => wb.Quantity);
            builder.HasOne(wb => wb.Wallet).WithMany(w => w.Balances).HasForeignKey(wb => wb.IdWallet);
            builder.HasOne(wb => wb.Token).WithMany(t => t.Balances).HasForeignKey(wb => wb.IdToken);
            builder.HasKey(cw => cw.ID);
        }
    }
}
