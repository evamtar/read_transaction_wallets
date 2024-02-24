using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class TokenSecurityReadyOnlyRepository : SqlServerReadyOnlyRepository<TokenSecurity>, ITokenSecurityReadyOnlyRepository
    {
        public TokenSecurityReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
