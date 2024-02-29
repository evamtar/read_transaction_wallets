using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using System.Linq.Expressions;

namespace SyncronizationBot.Infra.Data.MongoDB.Repository
{
    public class WalletBalanceRepository : MongoRepository<WalletBalance>, IWalletBalanceRepository
    {
        private MongoDbContext _dbContext;
        public WalletBalanceRepository(MongoDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Token>> GetAllTokensForUpdateAsync(Expression<Func<WalletBalance, bool>> expression) 
        {
            var listTokensForUpdate = await _dbContext.WalletBalances.Where(expression).Select(x => x.TokenId).Distinct().ToListAsync();
            return await _dbContext.Tokens.Where(x => listTokensForUpdate.Contains(x.ID)).ToListAsync();
        }

        public void UpdateAllBalancesWithToken(Token token) 
        {
            throw new NotImplementedException();
        }
    }
}
