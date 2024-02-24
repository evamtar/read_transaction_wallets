using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using SyncronizationBot.Infra.Data.SQLServer.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TokenAlphaWalletHistoryRepository : SqlServerRepository<TokenAlphaWalletHistory>, ITokenAlphaWalletHistoryRepository
    {
        public TokenAlphaWalletHistoryRepository(SqlServerContext context) : base(context)
        {

        }
    }
}
