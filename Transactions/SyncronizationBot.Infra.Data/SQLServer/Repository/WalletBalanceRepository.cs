using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using System.Linq.Expressions;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class WalletBalanceRepository : SqlServerRepository<WalletBalance>, IWalletBalanceRepository
    {
        public WalletBalanceRepository(SqlServerContext context) : base(context)
        {
        }

        public Task<List<Token>> GetAllTokensForUpdateAsync(Expression<Func<WalletBalance, bool>> expression) => throw new NotImplementedException();

        public void UpdateAllBalancesWithToken(Token token) => throw new NotImplementedException();
        
    }
}
