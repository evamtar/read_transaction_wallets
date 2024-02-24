using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using SyncronizationBot.Infra.Data.SQLServer.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TokenRepository : SqlServerRepository<Token>, ITokenRepository
    {
        public TokenRepository(SqlServerContext context) : base(context)
        {

        }
    }
}
