using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TokenRepository : SqlServerRepository<Token>, ITokenRepository
    {
        public TokenRepository(SqlServerContext context) : base(context)
        {
        }
    }
}
