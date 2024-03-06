using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.RecoveryService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using System.Linq.Expressions;
using SyncronizationBot.Domain.Repository.Base.Interfaces;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceService : CachedServiceBase<Entity.WalletBalance>, IWalletBalanceService
    {
        public WalletBalanceService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }

        public async Task<List<Entity.Token>> GetAllTokensForUpdateAsync(Expression<Func<Entity.WalletBalance, bool>> expression)
        {
            return await ((IWalletBalanceRepository)base._mongoRepository).GetAllTokensForUpdateAsync(expression);
        }

        public void UpdateAllBalancesWithToken(Entity.TokenPriceHistory tokenPriceHistory)
        {
            ((IWalletBalanceRepository)base._mongoRepository).UpdateAllBalancesWithToken(tokenPriceHistory);
        }
    }
}
