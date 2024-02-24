using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceHistoryService : CachedServiceBase<Entity.WalletBalanceHistory>, IWalletBalanceHistoryService
    {
        public WalletBalanceHistoryService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
