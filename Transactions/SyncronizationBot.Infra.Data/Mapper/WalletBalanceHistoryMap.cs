using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class WalletBalanceHistoryMap : IEntityTypeConfiguration<WalletBalanceHistory>
    {
        public void Configure(EntityTypeBuilder<WalletBalanceHistory> builder)
        {
            builder.ToTable("WalletBalanceHistory");
            builder.Property(wbh => wbh.ID);
            builder.Property(wbh => wbh.IdWalletBalance);
            builder.Property(wbh => wbh.IdWallet);
            builder.Property(wbh => wbh.IdToken);
            builder.Property(wbh => wbh.TokenHash); 
            builder.Property(wbh => wbh.OldQuantity).HasConversion<string?>();
            builder.Property(wbh => wbh.NewQuantity).HasConversion<string?>();
            builder.Property(wbh => wbh.RequestQuantity).HasConversion<string?>(); 
            builder.Property(wbh => wbh.PercentageCalculated).HasConversion<string?>();
            builder.Property(wbh => wbh.Price).HasConversion<string?>();
            builder.Property(wbh => wbh.TotalValueUSD).HasConversion<string?>();
            builder.Property(wbh => wbh.Signature);
            builder.Property(wbh => wbh.CreateDate);
            builder.Property(wbh => wbh.LastUpdate);
            builder.HasKey(cw => cw.ID);
        }
    }
}
