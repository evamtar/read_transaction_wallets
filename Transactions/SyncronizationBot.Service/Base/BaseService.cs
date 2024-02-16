using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Alerts;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;


namespace SyncronizationBot.Service.Base
{
    public abstract class BaseService : BackgroundService 
                           
    {
        protected readonly IMediator _mediator;
        protected readonly IRunTimeControllerRepository _runTimeControllerRepository;
        protected readonly ITypeOperationRepository _typeOperationRepository;
        protected readonly ETypeService _typeService;
        protected readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;

        #region For Alerts

        private TypeOperation? _typeOperationLogExecute;
        private TypeOperation? TypeOperationLogExecute 
        { 
            get 
            {
                if (this._typeOperationLogExecute == null)
                    this._typeOperationLogExecute = this._typeOperationRepository.FindFirstOrDefault(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogExecute).GetAwaiter().GetResult();
                return this._typeOperationLogExecute;
            } 
        }

        private TypeOperation? _typeOperationLogError;
        private TypeOperation? TypeOperationLogError
        {
            get
            {
                if (this._typeOperationLogError == null)
                    this._typeOperationLogError = this._typeOperationRepository.FindFirstOrDefault(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogError).GetAwaiter().GetResult();
                return this._typeOperationLogError;
            }
        }

        private TypeOperation? _typeOperationLogRunning;
        private TypeOperation? TypeOperationLogRunning
        {
            get
            {
                if (this._typeOperationLogRunning == null)
                    this._typeOperationLogRunning = this._typeOperationRepository.FindFirstOrDefault(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogAppRunning).GetAwaiter().GetResult();
                return this._typeOperationLogRunning;
            }
        }

        private TypeOperation? _typeOperationLogLostConfiguration;
        private TypeOperation? TypeOperationLogLostConfiguration
        {
            get
            {
                if (this._typeOperationLogLostConfiguration == null)
                    this._typeOperationLogLostConfiguration = this._typeOperationRepository.FindFirstOrDefault(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogAppLostConfiguration).GetAwaiter().GetResult();
                return this._typeOperationLogLostConfiguration;
            }
        }

        #endregion
        protected RunTimeController? RunTimeController { get; set; }
        protected PeriodicTimer Timer { get; set; }
        private int? TimesWithoutTransaction { get; set; }
        public bool? IsContingecyTransaction
        {
            get
            {
                return this.RunTimeController?.IsContingecyTransaction;
            }
        }

        public BaseService(IMediator mediator,
                           IRunTimeControllerRepository runTimeControllerRepository,
                           ITypeOperationRepository typeOperationRepository,
                           ETypeService typeService,
                           IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._runTimeControllerRepository = runTimeControllerRepository;
            this._typeOperationRepository = typeOperationRepository;
            this._typeService = typeService;
            this.Timer = null!;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        protected abstract Task DoExecute(CancellationToken cancellationToken);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
#if DEBUG
            if (this.GetType() == typeof(TestService))
            {
                await this.DoExecute(stoppingToken);
                return;
            }
#endif
            await this.SetPeriodicTimer();
            var hasNextExecute = this.Timer != null;
            if(hasNextExecute)
                this.LogMessage($"Inicialização do serviço: {this.RunTimeController?.JobName}");
            while (hasNextExecute)
            {
                try
                {
                    if (this.RunTimeController != null && (!this.RunTimeController!.IsRunning ?? true))
                    {
                        using var timer = this.Timer;
                        {
                            try
                            {
                                await this.SetRuntimeControllerAsync(true, false);
                                if (await timer!.WaitForNextTickAsync(stoppingToken)) 
                                {
                                    this.LogMessage($"Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                    await this.DoExecute(stoppingToken);
                                    await this.SetRuntimeControllerAsync(false, true);
                                    this.LogMessage($"End --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                    await this.SendAlertExecute(timer!);
                                    this.LogMessage($"Waiting for service {this.RunTimeController?.JobName} next tick in {timer!.Period}");
                                }
                            }
                            catch (Exception ex)
                            {
                                await this.SendAlertServiceError(ex, timer!);
                                await this.DetachedRuntimeControllerAsync();
                                await SetRuntimeControllerAsync(false, true);
                                this.LogMessage($"SERVICE -------------> : {this.RunTimeController?.JobName}");
                                this.LogMessage($"Exceção: {ex.Message}");
                                this.LogMessage($"StackTrace: {ex.StackTrace}");
                                this.LogMessage($"InnerException: {ex.InnerException}");
                                this.LogMessage($"InnerException---> Message: {ex.InnerException?.Message}");
                                this.LogMessage($"InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                                this.LogMessage($"Waiting for service {this.RunTimeController?.JobName} tick in {timer!.Period}");
                            }
                        }
                    }
                    else
                    {
                        this.LogMessage($"Is Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        await this.SendAlertServiceRunning();
                        await SetRuntimeControllerAsync(false, true);
                        await this.DetachedRuntimeControllerAsync();
                        this.LogMessage($"Recovery Service {this.RunTimeController?.JobName} ---> Waiting for next execute 00:30:00");
                        await Task.Delay(500);
                    }
                }
                catch(Exception ex) 
                {
                    this.LogMessage(ex.Message);
                }
                await this.SetPeriodicTimer();
                hasNextExecute = this.Timer != null;
            }
            await this.SendAlertTimerIsNull();
            this.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            this.LogMessage("Finalizado");
        }
        protected void EndTransactionsContingencySum(int? totalValidTransactions)
        {
            if (totalValidTransactions == 0)
                this.TimesWithoutTransaction += 1;
            else
                this.TimesWithoutTransaction = 0;
            this.RunTimeController!.TimesWithoutTransaction = this.TimesWithoutTransaction;
            if (this.TimesWithoutTransaction > this._syncronizationBotConfig.Value.MaxTimesWithoutTransactions)
            {
                if (!this._syncronizationBotConfig.Value.InValidation)
                    this.RunTimeController!.IsContingecyTransaction = !this.RunTimeController!.IsContingecyTransaction;
                this.RunTimeController!.TimesWithoutTransaction = 0;
            }
        }
        private async Task SetRuntimeControllerAsync(bool isRunning, bool detachedItem)
        {
            this.RunTimeController!.IsRunning = isRunning;
            this.RunTimeController = await this._runTimeControllerRepository.Edit(this.RunTimeController);
            if (detachedItem)
                this.RunTimeController = await DetachedRuntimeControllerAsync();
        }
        private async Task<RunTimeController?> DetachedRuntimeControllerAsync()
        {
            if(this.RunTimeController != null) 
                this.RunTimeController = await this._runTimeControllerRepository.DetachedItem(this.RunTimeController); 
            return this.RunTimeController;
        }
        private async Task SendAlertExecute(PeriodicTimer timer)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = timer.Period
                }}),
                TypeOperationId = this.TypeOperationLogExecute?.ID
            });
        }
        private async Task SendAlertServiceRunning()
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
                TypeOperationId = this.TypeOperationLogRunning?.ID
            });
        }
        private async Task SendAlertTimerIsNull()
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeOperationId = this.TypeOperationLogLostConfiguration?.ID
            });
        }
        private async Task SendAlertServiceError(Exception ex, PeriodicTimer timer)
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
                TypeOperationId = this.TypeOperationLogError?.ID
            });
        }
        private void LogMessage(string message) 
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
                case ETypeService.TransactionOldForMapping:
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
        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false && x.TypeService == this._typeService);
        }
        private void InitiParameterContingency()
        {
            this.TimesWithoutTransaction = this.RunTimeController?.TimesWithoutTransaction ?? 0;
        }
        private async Task SetPeriodicTimer()
        {
            if (this.RunTimeController == null)
            {
                this.RunTimeController = await this.GetRunTimeControllerAsync();
                this.InitiParameterContingency();
            }
            var minutesForTimeSpan = this.RunTimeController?.ConfigurationTimer ?? (decimal)1.00;
            this.Timer = new PeriodicTimer(TimeSpan.FromMinutes((double)minutesForTimeSpan));
        }
    }
}
