using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    internal class TokenAlphaHistoryMap : IEntityTypeConfiguration<TokenAlphaHistory>
    {
        public void Configure(EntityTypeBuilder<TokenAlphaHistory> builder)
        {
            builder.ToTable("TokenAlphaHistory");
            builder.Property(tah => tah.ID);
            builder.Property(tah => tah.TokenAlphaId); 
            builder.Property(tah => tah.CallNumber);
            builder.Property(tah => tah.InitialMarketcap).HasConversion<string?>();
            builder.Property(tah => tah.ActualMarketcap).HasConversion<string?>();
            builder.Property(tah => tah.InitialPrice).HasConversion<string?>();
            builder.Property(tah => tah.ActualPrice).HasConversion<string?>();
            builder.Property(tah => tah.RequestMarketCap).HasConversion<string?>();
            builder.Property(tah => tah.RequestPrice).HasConversion<string?>();
            builder.Property(tah => tah.CreateDate);
            builder.Property(tah => tah.LastUpdate);
            builder.Property(tah => tah.TokenId);
            builder.Property(tah => tah.TokenAlphaConfigurationId);
            builder.HasKey(tah => tah.ID);
        }
    }
}
