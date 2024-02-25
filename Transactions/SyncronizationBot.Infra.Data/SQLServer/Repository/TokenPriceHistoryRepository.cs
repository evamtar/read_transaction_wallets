using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TokenPriceHistoryRepository : SqlServerRepository<TokenPriceHistory>, ITokenPriceHistoryRepository
    {
        public TokenPriceHistoryRepository(SqlServerContext context) : base(context)
        {
        }
    }
}
