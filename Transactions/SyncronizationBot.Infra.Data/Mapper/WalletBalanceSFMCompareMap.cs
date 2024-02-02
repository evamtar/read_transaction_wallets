using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class WalletBalanceSFMCompareMap : IEntityTypeConfiguration<WalletBalanceSFMCompare>
    {
        public void Configure(EntityTypeBuilder<WalletBalanceSFMCompare> builder)
        {
            builder.ToTable("WalletBalanceSFMCompare");
            builder.Property(wbsfmc => wbsfmc.ID);
            builder.Property(wbsfmc => wbsfmc.WalletId);
            builder.Property(wbsfmc => wbsfmc.TokenId);
            builder.Property(wbsfmc => wbsfmc.TokenHash); 
            builder.Property(wbsfmc => wbsfmc.Quantity).HasConversion<string?>();
            builder.Property(wbsfmc => wbsfmc.Price).HasConversion<string?>();
            builder.Property(wbsfmc => wbsfmc.TotalValueUSD).HasConversion<string?>();
            builder.Property(wbsfmc => wbsfmc.IsActive); 
            builder.Property(wbsfmc => wbsfmc.LastUpdate);
            builder.HasOne(wbsfmc => wbsfmc.Wallet).WithMany(w => w.BalancesSFMCompare).HasForeignKey(wbsfmc => wbsfmc.WalletId);
            builder.HasOne(wbsfmc => wbsfmc.Token).WithMany(t => t.BalancesSFMCompare).HasForeignKey(wbsfmc => wbsfmc.TokenId);
            builder.HasKey(wbsfmc => wbsfmc.ID);
        }
    }
}
