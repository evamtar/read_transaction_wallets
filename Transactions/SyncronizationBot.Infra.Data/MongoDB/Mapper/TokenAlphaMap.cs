using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenAlphaMap : BaseMapper<TokenAlpha>
    {
        public TokenAlphaMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlpha> builder)
        {
            //builder.Property(ta => ta.InitialMarketcap).HasConversion<string?>();
            //builder.Property(ta => ta.ActualMarketcap).HasConversion<string?>();
            //builder.Property(ta => ta.InitialPrice).HasConversion<string?>();
            //builder.Property(ta => ta.ActualPrice).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenAlpha> builder)
        {
            builder.Ignore(ta => ta.Token);
            builder.Ignore(ta => ta.TokenAlphaConfiguration);
        }
        
    }
}
