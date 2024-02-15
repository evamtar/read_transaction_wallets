using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TokenAlphaWalletMap : IEntityTypeConfiguration<TokenAlphaWallet>
    {
        public void Configure(EntityTypeBuilder<TokenAlphaWallet> builder)
        {
            builder.ToTable("TokenAlphaWallet");
            builder.Property(taw => taw.ID);
            builder.Property(taw => taw.NumberOfBuys);
            builder.Property(taw => taw.ValueSpentSol).HasConversion<string?>();
            builder.Property(taw => taw.ValueSpentUSDC).HasConversion<string?>();
            builder.Property(taw => taw.ValueSpentUSDT).HasConversion<string?>();
            builder.Property(taw => taw.QuantityToken).HasConversion<string?>();
            builder.Property(taw => taw.NumberOfSells);
            builder.Property(taw => taw.ValueReceivedSol).HasConversion<string?>();
            builder.Property(taw => taw.ValueReceivedUSDC).HasConversion<string?>();
            builder.Property(taw => taw.ValueReceivedUSDT).HasConversion<string?>();
            builder.Property(taw => taw.QuantityTokenSell).HasConversion<string?>();
            builder.Property(taw => taw.TokenAlphaId);
            builder.Property(taw => taw.WalletId);
            builder.Property(taw => taw.WalletHash);
            builder.Property(taw => taw.ClassWalletDescription);
            builder.HasOne(taw => taw.TokenAlpha).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.TokenAlphaId);
            builder.HasOne(taw => taw.Wallet).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.WalletId);
            builder.HasKey(taw => taw.ID);
        }
    }
}
