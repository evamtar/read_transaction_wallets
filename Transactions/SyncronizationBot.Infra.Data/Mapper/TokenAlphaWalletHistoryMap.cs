using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TokenAlphaWalletHistoryMap : IEntityTypeConfiguration<TokenAlphaWalletHistory>
    {
        public void Configure(EntityTypeBuilder<TokenAlphaWalletHistory> builder)
        {
            builder.ToTable("TokenAlphaWalletHistory");
            builder.Property(tawh => tawh.ID);
            builder.Property(tawh => tawh.TokenAlphaWalletId);
            builder.Property(tawh => tawh.NumberOfBuys);
            builder.Property(tawh => tawh.ValueSpentSol).HasConversion<string?>();
            builder.Property(tawh => tawh.ValueSpentUSDC).HasConversion<string?>();
            builder.Property(tawh => tawh.ValueSpentUSDT).HasConversion<string?>();
            builder.Property(tawh => tawh.QuantityToken).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestValueInSol).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestValueInUSDC).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestValueInUSDT).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestQuantityToken).HasConversion<string?>();
            builder.Property(tawh => tawh.TokenAlphaId);
            builder.Property(tawh => tawh.WalletId);
            builder.Property(tawh => tawh.WalletHash);
            builder.Property(tawh => tawh.ClassWalletDescription);
            builder.HasKey(tawh => tawh.ID);
        }
    }
}
