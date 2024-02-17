using Entity = SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Utils;
using System.Collections.Concurrent;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Domain.Service.InternalService.Wallet;

namespace SyncronizationBot.Service.InternalServices.Utils
{
    public class PreLoadedEntitiesService : IPreLoadedEntitiesService
    {
        #region Readonly variables

        private readonly IRunTimeControllerService _runTimeControllerService;
        private readonly ITypeOperationService _typeOperationService;
        private readonly IClassWalletService _classWalletService;
        private readonly IWalletService _walletService;

        #endregion

        #region Properties

        private readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static DateTime LastUpdate { get; set; } = DateTime.Now;
        private static DateTime LastUpdateRunTimeController { get; set; } = DateTime.Now;
        private static TimeSpan IntervalUpdate { get; set; } = new TimeSpan(0, 0, 45);
        private static TimeSpan IntervalRunTimeControllerUpdate { get; set; } = new TimeSpan(0, 0, 15);

        #endregion

        #region Lists

        private static ConcurrentBag<Entity.TypeOperation>? TypesOperations { get; set; }
        private static ConcurrentBag<Entity.Wallet>? Wallets { get; set; }
        private static ConcurrentBag<Entity.RunTimeController>? RunTimeControllers { get; set; }
        private static ConcurrentBag<Entity.ClassWallet>? ClassWallets { get; set; }

        #endregion

        public PreLoadedEntitiesService(IRunTimeControllerService runTimeControllerService,
                                        ITypeOperationService typeOperationService,
                                        IClassWalletService classWalletService,
                                        IWalletService walletService)
        {
            this._typeOperationService = typeOperationService;
            this._classWalletService = classWalletService;
            this._runTimeControllerService = runTimeControllerService;
            this._walletService = walletService;
            SingleLoad().GetAwaiter().GetResult();
        }

        #region TypeOperation

        public async Task<List<Entity.TypeOperation>?> GetAllTypeOperationAsync()
        {
            await LoadLists();
            return TypesOperations?.ToList();
        }

        public async Task<List<Entity.TypeOperation>?> GetFilteredTypeOperationAsync(Func<Entity.TypeOperation, bool> predicate)
        {
            await LoadLists();
            return TypesOperations?.Where(predicate).ToList();
        }

        public async Task<Entity.TypeOperation?> FirstOrDefaulTypeOperationAsync(Func<Entity.TypeOperation, bool> predicate)
        {
            await LoadLists();
            return TypesOperations?.FirstOrDefault(predicate);
        }

        #endregion

        #region  RunTimeController

        public async Task<List<Entity.RunTimeController>?> GetAllRunTimeControllerAsync()
        {
            await LoadLists();
            return RunTimeControllers?.ToList();
        }
        public async Task<List<Entity.RunTimeController>?> GetFilteredRunTimeControllerAsync(Func<Entity.RunTimeController, bool> predicate)
        {
            await LoadLists();
            return RunTimeControllers?.Where(predicate).ToList();
        }
        public async Task<Entity.RunTimeController?> FirstOrDefaulRunTimeControllerAsync(Func<Entity.RunTimeController, bool> predicate)
        {
            await LoadLists();
            return RunTimeControllers?.FirstOrDefault(predicate);
        }

        #endregion

        #region  ClassWallet

        public async Task<List<Entity.ClassWallet>?> GetAllClassWalletAsync()
        {
            await LoadLists();
            return ClassWallets?.ToList();
        }
        public async Task<List<Entity.ClassWallet>?> GetFilteredClassWalletAsync(Func<Entity.ClassWallet, bool> predicate)
        {
            await LoadLists();
            return ClassWallets?.Where(predicate).ToList();
        }
        public async Task<Entity.ClassWallet?> FirstOrDefaulClassWalletAsync(Func<Entity.ClassWallet, bool> predicate)
        {
            await LoadLists();
            return ClassWallets?.FirstOrDefault(predicate);
        }

        #endregion

        #region  Wallets

        public async Task<List<Entity.Wallet>?> GetAllWalletAsync()
        {
            await LoadLists();
            return Wallets?.ToList();
        }
        public async Task<List<Entity.Wallet>?> GetFilteredWalletAsync(Func<Entity.Wallet, bool> predicate)
        {
            await LoadLists();
            return Wallets?.Where(predicate).ToList();
        }
        public async Task<Entity.Wallet?> FirstOrDefaulWalletAsync(Func<Entity.Wallet, bool> predicate)
        {
            await LoadLists();
            return Wallets?.FirstOrDefault(predicate);
        }

        #endregion

        #region Load Lists

        private async Task LoadLists()
        {
            await RefreshLoad();
        }

        private async Task SingleLoad()
        {
            if (TypesOperations?.Count == null || TypesOperations?.Count == 0 ||
                ClassWallets?.Count == null || ClassWallets.Count == 0)
            {
                await _semaphore.WaitAsync();
                try
                {
                    var taskList = new List<Task>();
                    // TypeOperation
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        TypesOperations = TypesOperations ?? new ConcurrentBag<Entity.TypeOperation>();
                        TypesOperations?.Clear();
                        using (var instanceService = this._typeOperationService)
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { TypesOperations?.Add(x); });
                            return Task.CompletedTask;
                        }
                    }).Unwrap());
                    // Class Wallet
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        ClassWallets = ClassWallets ?? new ConcurrentBag<Entity.ClassWallet>();
                        ClassWallets?.Clear();
                        using (var instanceService = this._classWalletService)
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { ClassWallets?.Add(x); });
                            return Task.CompletedTask;
                        };
                    }).Unwrap());
                    await Task.WhenAll(taskList);
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        private async Task RefreshLoad()
        {
            //General Interval
            if (((DateTime.Now - LastUpdate) >= IntervalUpdate) || Wallets?.Count == null || Wallets?.Count == 0)
            {
                await _semaphore.WaitAsync();
                try
                {

                    var taskList = new List<Task>();
                    // Wallets
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        Wallets = Wallets ?? new ConcurrentBag<Entity.Wallet>();
                        Wallets?.Clear();
                        using (var instanceService = this._walletService)
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { Wallets?.Add(x); });
                            return Task.CompletedTask;
                        };
                    }).Unwrap());
                    await Task.WhenAll(taskList);
                }
                finally
                {
                    _semaphore.Release();
                }
                LastUpdate = DateTime.Now;
            }
            //Person Interval
            if (((DateTime.Now - LastUpdateRunTimeController) >= IntervalRunTimeControllerUpdate) || RunTimeControllers?.Count == null || RunTimeControllers?.Count == 0)
            {
                await _semaphore.WaitAsync();
                try
                {
                    //Runtime
                    using (var runTimeControllerService = this._runTimeControllerService)
                    {
                        RunTimeControllers = RunTimeControllers ?? new ConcurrentBag<Entity.RunTimeController>();
                        RunTimeControllers?.Clear();
                        using (var instanceService = runTimeControllerService)
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { RunTimeControllers?.Add(x); });
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
                LastUpdateRunTimeController = DateTime.Now;
            }
        }

        #endregion
    }
}
