using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenPriceHistoryMap : BaseMapper<TokenPriceHistory>
    {
        public TokenPriceHistoryMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenPriceHistory> builder)
        {
            builder.HasOne(tph => tph.Token).WithMany(t => t.TokenPriceHistories).HasForeignKey(tph => tph.TokenId);
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenPriceHistory> builder)
        {
            builder.Property(tph => tph.MarketCap).HasConversion<string?>();
            builder.Property(tph => tph.Liquidity).HasConversion<string?>();
        }

    }
}
