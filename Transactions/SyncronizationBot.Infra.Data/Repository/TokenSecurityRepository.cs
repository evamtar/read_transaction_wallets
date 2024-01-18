using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Infra.Data.Context;
using SyncronizationBot.Infra.Data.Repository.Base;

namespace SyncronizationBot.Infra.Data.Repository
{
    public class TokenSecurityRepository : Repository<TokenSecurity>, ITokenSecurityRepository
    {
        public TokenSecurityRepository(SqlContext context) : base(context)
        {

        }
    }
}
