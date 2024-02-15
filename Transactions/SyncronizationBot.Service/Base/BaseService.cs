using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Alerts;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;


namespace SyncronizationBot.Service.Base
{
    public abstract class BaseService : BackgroundService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        protected readonly IMediator _mediator;
        protected readonly IRunTimeControllerRepository _runTimeControllerRepository;
        protected readonly ETypeService _typeService;
        protected readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        protected RunTimeController? RunTimeController { get; set; }
        protected PeriodicTimer Timer { get; set; }
        private int? TimesWithoutTransactions { get; set; }
        public bool? IsContingecyTransactions
        {
            get
            {
                return this.RunTimeController?.IsContingecyTransactions;
            }
        }

        public BaseService(IMediator mediator,
                           IRunTimeControllerRepository runTimeControllerRepository,
                           ETypeService typeService,
                           IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._runTimeControllerRepository = runTimeControllerRepository;
            this._typeService = typeService;
            this.Timer = null!;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        protected abstract Task DoExecute(PeriodicTimer timer, CancellationToken stoppingToken);
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
                #if DEBUG
                if (this.GetType() == typeof(TestService)) 
                {
                    await this.DoExecute(this.Timer!, stoppingToken);
                    return;
                }
                #endif
                await this.SetPeriodicTimer();
                var hasNextExecute = this.Timer != null;
                while (hasNextExecute)
                {

                    using var timer = this.Timer;
                    {
                        try 
                        { 
                            if (await timer!.WaitForNextTickAsync(stoppingToken))
                                await this.DoExecute(this.Timer!, stoppingToken);
                        } 
                        catch { }
                    }
                    await this.SetPeriodicTimer();
                    hasNextExecute = this.Timer != null;
                }
                await this.SendAlertTimerIsNull();
                this.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                this.LogMessage("Finalizado");
        }

        protected async Task SetRuntimeControllerAsync(bool isRunning, bool detachedItem)
        {
            this.RunTimeController!.IsRunning = isRunning;
            this.RunTimeController = await this._runTimeControllerRepository.Edit(this.RunTimeController);
            if (detachedItem)
                this.RunTimeController = await DetachedRuntimeControllerAsync();
        }

        protected async Task<RunTimeController?> DetachedRuntimeControllerAsync()
        {
            if(this.RunTimeController != null) 
                this.RunTimeController = await this._runTimeControllerRepository.DetachedItem(this.RunTimeController); 
            return this.RunTimeController;
        }

        protected async Task SendAlertExecute(PeriodicTimer timer)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = timer.Period
                }}),
                TypeAlert = ETypeAlert.LOG_EXECUTE
            });
        }

        protected async Task SendAlertServiceRunning()
        {
            await this.DetachedRuntimeControllerAsync();
            this.RunTimeController = await this.GetRunTimeControllerAsync();
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeAlert = ETypeAlert.LOG_APP_RUNNING
            });
        }

        protected async Task SendAlertTimerIsNull()
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeAlert = ETypeAlert.LOG_LOST_CONFIGURATION
            });
        }

        protected async Task SendAlertServiceError(Exception ex, PeriodicTimer timer)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = timer.Period,
                    Exception = ex,
                }}),
                TypeAlert = ETypeAlert.LOG_ERROR
            });
        }

        protected void LogMessage(string message) 
        {
            switch (this._typeService)
            {
                case ETypeService.Transaction:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ETypeService.Balance:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ETypeService.Price:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ETypeService.DeleteOldMessages:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ETypeService.AlertTokenAlpha:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ETypeService.TransactionsOldForMapping:
                    Console.ForegroundColor = ConsoleColor.Magenta; 
                    break;
                case ETypeService.NewTokensBetAwards:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    break;
            }
            Console.WriteLine(message);
        }

        protected void EndTransactionsContingencySum(int? totalValidTransactions)
        {
            if (totalValidTransactions == 0)
                this.TimesWithoutTransactions += 1;
            else
                this.TimesWithoutTransactions = 0;
            this.RunTimeController!.TimesWithoutTransactions = this.TimesWithoutTransactions;
            if (this.TimesWithoutTransactions > this._syncronizationBotConfig.Value.MaxTimesWithoutTransactions) 
            {
                if(!this._syncronizationBotConfig.Value.InValidation)
                  this.RunTimeController!.IsContingecyTransactions = !this.RunTimeController!.IsContingecyTransactions;
                this.RunTimeController!.TimesWithoutTransactions = 0;
            }
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false && x.TypeService == this._typeService);
        }

        private void InitiParameterContingency()
        {
            this.TimesWithoutTransactions = this.RunTimeController?.TimesWithoutTransactions ?? 0;
        }
        private async Task SetPeriodicTimer()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (this.RunTimeController == null)
                {
                    this.RunTimeController = await this.GetRunTimeControllerAsync();
                    this.InitiParameterContingency();
                }
                var minutesForTimeSpan = this.RunTimeController?.ConfigurationTimer ?? (decimal)1.00;
                this.Timer = new PeriodicTimer(TimeSpan.FromMinutes((double)minutesForTimeSpan));
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
