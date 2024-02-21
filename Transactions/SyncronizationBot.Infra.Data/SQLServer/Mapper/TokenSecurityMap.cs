using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenSecurityMap : BaseMapper<TokenSecurity>
    {
        public TokenSecurityMap() : base(EDatabase.SqlServer)
        {

        }

        protected override void RelationsShips(EntityTypeBuilder<TokenSecurity> builder)
        {
            builder.HasOne(ts => ts.Token).WithMany(t => t.TokenSecurities).HasForeignKey(ts => ts.TokenId);
        }
        
    }
}
