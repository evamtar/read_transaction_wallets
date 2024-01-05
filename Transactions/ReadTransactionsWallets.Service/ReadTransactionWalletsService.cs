using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Utils;

namespace ReadTransactionsWallets.Service
{
    public class ReadTransactionWalletsService: BackgroundService
    {
        private readonly IOptions<ReadTransactionWalletsConfig> _options;
        private readonly IRunTimeControllerRepository _runTimeControllerRepository;
        public ReadTransactionWalletsService(IOptions<ReadTransactionWalletsConfig> options, IRunTimeControllerRepository runTimeControllerRepository)
        {
            this._options = options;
            this._runTimeControllerRepository = runTimeControllerRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Iniciando o bot de leitura de transações efetuadas nas wallets mapeadas");
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(this._options.Value.ConfigurationTimer ?? 5));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var runtimeController = await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false);
                if (runtimeController != null && (!runtimeController!.IsRunning ?? true))
                {
                    Console.WriteLine("Init Read: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    var initialTicks = runtimeController?.UnixTimeSeconds ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now.AddDays(-1));
                    var finalTicks = DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now);
                    runtimeController = await SetRuntimeController(runtimeController!, true, initialTicks, false);
                    Console.WriteLine("Initial Ticks: " + initialTicks.ToString());
                    Console.WriteLine("Final Ticks: " + finalTicks.ToString());
                    runtimeController = await SetRuntimeController(runtimeController!, false, finalTicks, true);
                    Console.WriteLine("End Read: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    Console.WriteLine($"Waiting for next tick in {timer.Period}");
                }
                else 
                {
                    runtimeController = await DetachedRuntimeController(runtimeController!);
                    Console.WriteLine("Aplicativo rodando" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }
            }
            Console.WriteLine("Finalizado");
            return;
        }

        private async Task<RunTimeController> SetRuntimeController(RunTimeController runTimeController, bool isRunning, decimal unixTimeSeconds, bool detachedItem) 
        {
            runTimeController.IsRunning = isRunning;
            runTimeController.UnixTimeSeconds = unixTimeSeconds;
            runTimeController = await this._runTimeControllerRepository.Edit(runTimeController);
            if (detachedItem)
                runTimeController = await DetachedRuntimeController(runTimeController);
            return runTimeController;
        }

        private async Task<RunTimeController> DetachedRuntimeController(RunTimeController runTimeController)
        {
            runTimeController = await this._runTimeControllerRepository.DetachedItem(runTimeController);
            return runTimeController;
        }
        
    }
}
