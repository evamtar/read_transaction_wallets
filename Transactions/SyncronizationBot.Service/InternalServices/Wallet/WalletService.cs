using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository.UnitOfWork;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletService : CachedServiceBase<Entity.Wallet>, IWalletService
    {
        public WalletService(IUnitOfWorkSqlServerReadyOnly unitOfWorkSqlServerReadyOnly, IUnitOfWorkMongo unitOfWorkMongo) : base(unitOfWorkSqlServerReadyOnly, unitOfWorkMongo)
        {
        }
    }
}
