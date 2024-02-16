using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.RecoveryService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletBalanceService : ServiceBase<Entity.WalletBalance>, IWalletBalanceService
    {
        public WalletBalanceService(IWalletBalanceRepository repository) : base(repository)
        {
        }
    }
}
