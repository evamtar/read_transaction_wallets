using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using System.Linq.Expressions;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class WalletBalanceRepository : SqlServerRepository<WalletBalance>, IWalletBalanceRepository
    {
        private SqlServerContext _dbContext;
        public WalletBalanceRepository(SqlServerContext context) : base(context)
        {
            _dbContext = context;
        }

        public Task<List<Token>> GetAllTokensForUpdateAsync(Expression<Func<WalletBalance, bool>> expression) => throw new NotImplementedException();

        public void UpdateAllBalancesWithToken(TokenPriceHistory tokenPriceHistory) 
        {
            _dbContext.WalletBalances.Where(x => x.TokenId == tokenPriceHistory.TokenId).ExecuteUpdate(wbl => wbl.SetProperty(w => w.TotalValueUSD, w => w.Quantity * tokenPriceHistory.PriceUsd));
        }
        
    }
}
