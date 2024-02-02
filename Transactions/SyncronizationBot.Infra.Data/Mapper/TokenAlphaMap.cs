using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    internal class TokenAlphaMap : IEntityTypeConfiguration<TokenAlpha>
    {
        public void Configure(EntityTypeBuilder<TokenAlpha> builder)
        {
            builder.ToTable("TokenAlpha");
            builder.Property(ta => ta.ID);
            builder.Property(ta => ta.TokenId);
            builder.Property(ta => ta.CallNumber);
            builder.Property(ta => ta.InitialMarketcap).HasConversion<string?>();
            builder.Property(ta => ta.ActualMarketcap).HasConversion<string?>();
            builder.Property(ta => ta.InitialPrice).HasConversion<string?>();
            builder.Property(ta => ta.ActualPrice).HasConversion<string?>();
            builder.Property(ta => ta.CreateDate);
            builder.Property(ta => ta.LastUpdate);
            builder.Property(ta => ta.IsCalledInChannel);
            builder.HasOne(ta => ta.Token).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.TokenId);
            builder.HasKey(ta => ta.ID);
        }
    }
}
