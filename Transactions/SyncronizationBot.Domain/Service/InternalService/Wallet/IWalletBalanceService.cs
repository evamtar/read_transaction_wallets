using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Service.InternalService.Base;
using System.Linq.Expressions;

namespace SyncronizationBot.Domain.Service.RecoveryService.Wallet
{
    public interface IWalletBalanceService : ICachedServiceBase<WalletBalance>
    {
        Task<List<Token>> GetAllTokensForUpdateAsync(Expression<Func<WalletBalance, bool>> expression);
    }
}
