using SyncronizationBot.Domain.Model.Database.Base;
using Entity = SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Domain.Service.InternalService.Utils
{
    public interface IPreLoadedEntitiesService
    {
        Task<List<Entity.TypeOperation>?> GetAllTypeOperationAsync();
        Task<List<Entity.TypeOperation>?> GetFilteredTypeOperationAsync(Func<Entity.TypeOperation, bool> predicate);
        Task<Entity.TypeOperation?> FirstOrDefaulTypeOperationAsync(Func<Entity.TypeOperation, bool> predicate);

        Task<List<Entity.RunTimeController>?> GetAllRunTimeControllerAsync();
        Task<List<Entity.RunTimeController>?> GetFilteredRunTimeControllerAsync(Func<Entity.RunTimeController, bool> predicate);
        Task<Entity.RunTimeController?> FirstOrDefaulRunTimeControllerAsync(Func<Entity.RunTimeController, bool> predicate);

        Task<List<Entity.ClassWallet>?> GetAllClassWalletAsync();
        Task<List<Entity.ClassWallet>?> GetFilteredClassWalletAsync(Func<Entity.ClassWallet, bool> predicate);
        Task<Entity.ClassWallet?> FirstOrDefaulClassWalletAsync(Func<Entity.ClassWallet, bool> predicate);

        Task<List<Entity.Wallet>?> GetAllWalletAsync();
        Task<List<Entity.Wallet>?> GetFilteredWalletAsync(Func<Entity.Wallet, bool> predicate);
        Task<Entity.Wallet?> FirstOrDefaulWalletAsync(Func<Entity.Wallet, bool> predicate);
    }
}
