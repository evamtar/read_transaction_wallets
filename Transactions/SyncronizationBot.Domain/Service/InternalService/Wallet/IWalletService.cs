using SyncronizationBot.Domain.Service.RecoveryService.Base;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Domain.Service.InternalService.Wallet
{
    public interface IWalletService : IServiceBase<Entity.Wallet>
    {
    }
}
