using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenPriceHistoryMap : BaseMapper<TokenPriceHistory>
    {
        public TokenPriceHistoryMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenPriceHistory> builder)
        {
            builder.Ignore(tph => tph.Token);
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenPriceHistory> builder)
        {
            //builder.Property(tph => tph.MarketCap).HasConversion<string?>();
            //builder.Property(tph => tph.Liquidity).HasConversion<string?>();
        }

    }
}
