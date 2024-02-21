using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenSecurityMap : BaseMapper<TokenSecurity>
    {
        public TokenSecurityMap() : base(EDatabase.Mongodb)
        {

        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenSecurity> builder)
        {
            //builder.Property(ts => ts.Top10HolderBalance).HasConversion<string?>();
            //builder.Property(ts => ts.Top10HolderPercent).HasConversion<string?>();
            //builder.Property(ts => ts.Top10UserBalance).HasConversion<string?>();
            //builder.Property(ts => ts.Top10UserPercent).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<TokenSecurity> builder)
        {
            builder.Ignore(ts => ts.Token);
        }
        
    }
}
