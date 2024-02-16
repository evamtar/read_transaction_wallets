using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using System.Collections.Concurrent;

namespace SyncronizationBot.Domain.Model.PreLoaded
{
    public static class PreLoadedEntities
    {
        #region Properties

        private readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static DateTime LastUpdateTime { get; set; } = DateTime.Now;
        private static TimeSpan IntervalUpdate { get; set; } = new TimeSpan(0, 0, 45);
        private static TimeSpan IntervalRunTimeControllerUpdate { get; set; } = new TimeSpan(0, 0, 15);
        public static IServiceProvider? ServiceProvider { get; set; }

        #endregion

        #region Lists

        private static ConcurrentBag<TypeOperation>? TypesOperations { get; set; }
        private static ConcurrentBag<Wallet>? Wallets { get; set; }
        private static ConcurrentBag<RunTimeController>? RunTimeControllers { get; set; }
        private static ConcurrentBag<ClassWallet>? ClassWallets { get; set; }

        #endregion

        #region TypeOperation

        public static async Task<List<TypeOperation>?> GetAllTypeOperationAsync() 
        {
            await LoadLists();
            return TypesOperations?.ToList();
        }

        public static async Task<List<TypeOperation>?> GetFilteredTypeOperationAsync(Func<TypeOperation, bool> predicate)
        {
            await LoadLists();
            return TypesOperations?.Where(predicate).ToList();
        }

        public static async Task<TypeOperation?> FirstOrDefaulTypeOperationAsync(Func<TypeOperation, bool> predicate)
        {
            await LoadLists();
            return TypesOperations?.FirstOrDefault(predicate);
        }

        #endregion

        #region  RunTimeController

        public static async Task<List<RunTimeController>?> GetAllRunTimeControllerAsync()
        {
            await LoadLists();
            return RunTimeControllers?.ToList();
        }
        public static async Task<List<RunTimeController>?> GetFilteredRunTimeControllerAsync(Func<RunTimeController, bool> predicate)
        {
            await LoadLists();
            return RunTimeControllers?.Where(predicate).ToList();
        }
        public static async Task<RunTimeController?> FirstOrDefaulRunTimeControllerAsync(Func<RunTimeController, bool> predicate)
        {
            await LoadLists();
            return RunTimeControllers?.FirstOrDefault(predicate);
        }

        #endregion

        #region  ClassWallet

        public static async Task<List<ClassWallet>?> GetAllClassWalletAsync()
        {
            await LoadLists();
            return ClassWallets?.ToList();
        }
        public static async Task<List<ClassWallet>?> GetFilteredClassWalletAsync(Func<ClassWallet, bool> predicate)
        {
            await LoadLists();
            return ClassWallets?.Where(predicate).ToList();
        }
        public static async Task<ClassWallet?> FirstOrDefaulClassWalletAsync(Func<ClassWallet, bool> predicate)
        {
            await LoadLists();
            return ClassWallets?.FirstOrDefault(predicate);
        }

        #endregion

        #region  Wallets

        public static async Task<List<Wallet>?> GetAllWalletAsync()
        {
            await LoadLists();
            return Wallets?.ToList();
        }
        public static async Task<List<Wallet>?> GetFilteredWalletAsync(Func<Wallet, bool> predicate)
        {
            await LoadLists();
            return Wallets?.Where(predicate).ToList();
        }
        public static async Task<Wallet?> FirstOrDefaulWalletAsync(Func<Wallet, bool> predicate)
        {
            await LoadLists();
            return Wallets?.FirstOrDefault(predicate);
        }

        #endregion

        #region Load Lists

        private static async Task LoadLists()
        {
            if (((DateTime.Now - LastUpdateTime) >= IntervalUpdate) || Wallets?.Count < 0)
            {
                await _semaphore.WaitAsync();
                try
                {
                    var taskList = new List<Task>();
                    // Type Operation
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        TypesOperations = TypesOperations ?? new ConcurrentBag<TypeOperation>();
                        TypesOperations?.Clear();
                        using (var instanceService = (ITypeOperationService?)ServiceProvider?.GetService(typeof(ITypeOperationService)))
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { TypesOperations?.Add(x); });
                            return Task.CompletedTask;
                        }
                    }));
                    // Class Wallet
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        ClassWallets = ClassWallets ?? new ConcurrentBag<ClassWallet>();
                        ClassWallets?.Clear();
                        using (var instanceService = (IClassWalletService?)ServiceProvider?.GetService(typeof(IClassWalletService)))
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { ClassWallets?.Add(x); });
                            return Task.CompletedTask;
                        };
                    }));
                    // Wallets
                    taskList.Add(Task.Factory.StartNew(async () =>
                    {
                        Wallets = Wallets ?? new ConcurrentBag<Wallet>();
                        Wallets?.Clear();
                        using (var instanceService = (IWalletService?)ServiceProvider?.GetService(typeof(IWalletService)))
                        {
                            var list = await instanceService?.GetAll();
                            list.ForEach(x => { Wallets?.Add(x); });
                            return Task.CompletedTask;
                        };
                    }));
                    Task.WaitAll(Task.WhenAll(taskList));
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            if (((DateTime.Now - LastUpdateTime) >= IntervalRunTimeControllerUpdate) || RunTimeControllers?.Count < 0) 
            {
                await _semaphore.WaitAsync();
                try
                {
                    //Runtime
                    RunTimeControllers = RunTimeControllers ?? new ConcurrentBag<RunTimeController>();
                    RunTimeControllers?.Clear();
                    using (var instanceService = (IRunTimeControllerService?)ServiceProvider?.GetService(typeof(IRunTimeControllerService)))
                    {
                        var list = await instanceService?.GetAll();
                        list.ForEach(x => { RunTimeControllers?.Add(x); });
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        #endregion
    }
}
