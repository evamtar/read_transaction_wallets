using SyncronizationBot.Domain.Model.Database;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Repository.Base.Interfaces
{
    public interface IWalletBalanceRepository : IRepository<WalletBalance>
    {
        Task<List<Token>> GetAllTokensForUpdateAsync(Expression<Func<WalletBalance, bool>> expression);
        void UpdateAllBalancesWithToken(TokenPriceHistory tokenPriceHistory);
    }
}
