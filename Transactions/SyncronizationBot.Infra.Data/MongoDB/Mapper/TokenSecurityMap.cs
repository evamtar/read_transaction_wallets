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

        protected override void IgnoreProperties(EntityTypeBuilder<TokenSecurity> builder)
        {
            builder.Ignore(ts => ts.Token);
        }
        
    }
}
