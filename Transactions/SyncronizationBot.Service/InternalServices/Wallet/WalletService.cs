using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Service.InternalServices.Base;
using Entity = SyncronizationBot.Domain.Model.Database;
namespace SyncronizationBot.Service.InternalServices.Wallet
{
    public class WalletService : ServiceBase<Entity.Wallet>, IWalletService
    {
        public WalletService(IWalletRepository repository) : base(repository)
        {
        }
    }
}
