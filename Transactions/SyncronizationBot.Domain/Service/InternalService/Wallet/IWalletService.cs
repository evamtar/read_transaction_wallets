using SyncronizationBot.Domain.Service.InternalService.Base;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Domain.Service.InternalService.Wallet
{
    public interface IWalletService : ICachedServiceBase<Entity.Wallet>
    {
    }
}
