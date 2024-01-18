using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TokenMap : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable("Token");
            builder.Property(t => t.ID);
            builder.Property(t => t.Hash);
            builder.Property(t => t.Symbol);
            builder.Property(t => t.Name);
            builder.Property(t => t.Supply).HasPrecision(38, 10).HasConversion<decimal?>();
            builder.Property(t => t.MarketCap);
            builder.Property(t => t.Liquidity);
            builder.Property(t => t.UniqueWallet24h);
            builder.Property(t => t.UniqueWalletHistory24h);
            builder.Property(t => t.Decimals);
            builder.Property(t => t.NumberMarkets);
            builder.Property(t => t.CreateDate);
            builder.Property(t => t.LastUpdate);
            builder.HasKey(t => t.ID);
        }
    }
}
