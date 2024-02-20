using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    internal class TokenAlphaHistoryMap : BaseMapper<TokenAlphaHistory>
    {
        public TokenAlphaHistoryMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlphaHistory> builder)
        {
            builder.Property(tah => tah.InitialMarketcap).HasConversion<string?>();
            builder.Property(tah => tah.ActualMarketcap).HasConversion<string?>();
            builder.Property(tah => tah.InitialPrice).HasConversion<string?>();
            builder.Property(tah => tah.ActualPrice).HasConversion<string?>();
            builder.Property(tah => tah.RequestMarketCap).HasConversion<string?>();
            builder.Property(tah => tah.RequestPrice).HasConversion<string?>();
        }
        
    }
}
