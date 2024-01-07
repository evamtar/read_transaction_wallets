using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReadTransactionsWallets.Application.Commands;
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
        private readonly IMediator _mediator;
        public ReadTransactionWalletsService(IMediator mediator,
                                             IOptions<ReadTransactionWalletsConfig> options, 
                                             IRunTimeControllerRepository runTimeControllerRepository)
        {
            this._mediator = mediator;
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
                Console.WriteLine($"Init Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                var initialTicks = runtimeController?.UnixTimeSeconds ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now.AddDays(-1));
                if (runtimeController != null && (!runtimeController!.IsRunning ?? true))
                {
                    try
                    {
                        var finalTicks = DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now);
                        runtimeController = await SetRuntimeController(runtimeController!, true, initialTicks, false);
                        Console.WriteLine($"Initial Ticks: {initialTicks}");
                        Console.WriteLine($"Final Ticks: {finalTicks}");
                        await this._mediator.Send(new ReadWalletsCommand { InitialTicks = initialTicks, FinalTicks = finalTicks });
                        runtimeController = await SetRuntimeController(runtimeController!, false, finalTicks, true);
                        Console.WriteLine($"End Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        await this._mediator.Send(new SendTelegramMessageCommand { Channel = EChannel.CallSolanaLog, Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_EXECUTE, new object[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), timer.Period }) });
                        Console.WriteLine($"Waiting for next tick in {timer.Period}");
                    }
                    catch (Exception ex)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand { Channel = EChannel.CallSolanaLog, Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_EXECUTE_ERROR, new object[] { ex.Message, ex.StackTrace?? string.Empty, timer.Period }) });
                        Console.WriteLine($"Exceção: {ex.Message}");
                        Console.WriteLine($"StackTrace: {ex.StackTrace}");
                        Console.WriteLine($"Waiting for next tick in {timer.Period}");
                    }
                    finally 
                    {
                        runtimeController = await SetRuntimeController(runtimeController!, false, initialTicks, true);
                    }
                }
                else
                {
                    await this._mediator.Send(new SendTelegramMessageCommand { Channel = EChannel.CallSolanaLog, Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_APP_RUNNING, new object[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), timer.Period }) });
                    Console.WriteLine($"Aplicativo rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
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
            try { runTimeController = await this._runTimeControllerRepository.DetachedItem(runTimeController); }
            catch { }//NOTHING HERE 
            return runTimeController;
        }
        
    }
}

