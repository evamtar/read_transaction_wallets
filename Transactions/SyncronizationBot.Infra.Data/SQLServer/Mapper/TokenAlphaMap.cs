using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenAlphaMap : BaseMapper<TokenAlpha>
    {
        public TokenAlphaMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlpha> builder)
        {
            builder.Property(ta => ta.InitialMarketcap).HasConversion<string?>();
            builder.Property(ta => ta.ActualMarketcap).HasConversion<string?>();
            builder.Property(ta => ta.InitialPrice).HasConversion<string?>();
            builder.Property(ta => ta.ActualPrice).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenAlpha> builder)
        {
            builder.HasOne(ta => ta.Token).WithMany(t => t.TokenAlphas).HasForeignKey(ta => ta.TokenId);
            builder.HasOne(ta => ta.TokenAlphaConfiguration).WithMany(tac => tac.TokenAlphas).HasForeignKey(ta => ta.TokenAlphaConfigurationId);
        }
        
    }
}
