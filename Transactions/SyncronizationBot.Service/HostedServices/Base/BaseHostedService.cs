using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.UpdateCommand.RunTimeController.Command;
using SyncronizationBot.Domain.Model.Alerts;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.InternalService.HostedWork.Base;
using SyncronizationBot.Domain.Service.InternalService.Utils;

namespace SyncronizationBot.Service.HostedServices.Base
{
    public class BaseHostedService<T> : BackgroundService where T : IHostWorkService

    {
        #region Readonly Variables

        private readonly IServiceProvider _serviceProvider;
        
        #endregion

        #region For Alerts

        private TypeOperation? _typeOperationLogExecute;
        private TypeOperation? TypeOperationLogExecute
        {
            get
            {
                if (_typeOperationLogExecute == null)
                    _typeOperationLogExecute = this.PreLoadedEntitiesService?.FirstOrDefaulTypeOperationAsync(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogExecute).GetAwaiter().GetResult();
                return _typeOperationLogExecute;
            }
        }

        private TypeOperation? _typeOperationLogError;
        private TypeOperation? TypeOperationLogError
        {
            get
            {
                if (_typeOperationLogError == null)
                    this._typeOperationLogError = this.PreLoadedEntitiesService?.FirstOrDefaulTypeOperationAsync(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogError).GetAwaiter().GetResult();
                return this._typeOperationLogError;
            }
        }

        private TypeOperation? _typeOperationLogRunning;
        private TypeOperation? TypeOperationLogRunning
        {
            get
            {
                if (_typeOperationLogRunning == null)
                    _typeOperationLogRunning = this.PreLoadedEntitiesService?.FirstOrDefaulTypeOperationAsync(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogAppRunning).GetAwaiter().GetResult();
                return _typeOperationLogRunning;
            }
        }

        private TypeOperation? _typeOperationLogLostConfiguration;
        private TypeOperation? TypeOperationLogLostConfiguration
        {
            get
            {
                if (_typeOperationLogLostConfiguration == null)
                    _typeOperationLogLostConfiguration = this.PreLoadedEntitiesService?.FirstOrDefaulTypeOperationAsync(x => x.IdTypeOperation == (int)EFixedTypeOperation.LogAppLostConfiguration).GetAwaiter().GetResult();
                return _typeOperationLogLostConfiguration;
            }
        }

        #endregion

        #region Properties

        protected IMediator Mediator { get; set; }
        protected T? Work { get; set; }
        protected IPreLoadedEntitiesService? PreLoadedEntitiesService { get; set; }
        protected IOptions<SyncronizationBotConfig>? Options { get; set; }
        protected RunTimeController? RunTimeController { get; set; }
        protected PeriodicTimer Timer { get; set; }
        private int? TimesWithoutTransaction { get; set; }

        /// <summary>
        /// TODO-EVANDRO
        /// </summary>
        public bool? IsContingecyTransaction
        {
            get
            {
                return RunTimeController?.IsContingecyTransaction;
            }
        }

        #endregion

        #region Constructors

        public BaseHostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.PreLoadedEntitiesService = null!;
            this.Options = null!;
            this.Timer = null!;
            this.Mediator = null!;
            this.Work = default;
        }

        #endregion

        
        #region ExecuteAsync Override

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = this._serviceProvider.CreateScope()) 
            {
                this.PreLoadedEntitiesService = scope.ServiceProvider.GetRequiredService<IPreLoadedEntitiesService>();
                this.Work = scope.ServiceProvider.GetRequiredService<T>();
                this.Mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                //Init Service
                await SetPeriodicTimer();
                var hasNextExecute = Timer != null;
                if (hasNextExecute)
                    LogMessage($"Inicialização do serviço: {this.RunTimeController?.JobName}");
                while (!stoppingToken.IsCancellationRequested && hasNextExecute)
                {
                    try
                    {
                        if (this.RunTimeController != null && (!this.RunTimeController!.IsRunning ?? true))
                        {
                            using var timer = Timer;
                            {
                                try
                                {
                                    await SetRuntimeControllerAsync(true);
                                    if (await timer!.WaitForNextTickAsync(stoppingToken))
                                    {
                                        LogMessage($"Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                        await this.Work.DoExecute(stoppingToken);
                                        await SetRuntimeControllerAsync(false);
                                        LogMessage($"End --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                        await SendAlertExecute(timer!);
                                        LogMessage($"Waiting for service {this.RunTimeController?.JobName} next tick in {timer!.Period}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await SendAlertServiceError(ex, timer!);
                                    await SetRuntimeControllerAsync(false);
                                    LogMessage($"SERVICE -------------> : {this.RunTimeController?.JobName}");
                                    LogMessage($"Exceção: {ex.Message}");
                                    LogMessage($"StackTrace: {ex.StackTrace}");
                                    LogMessage($"InnerException: {ex.InnerException}");
                                    LogMessage($"InnerException---> Message: {ex.InnerException?.Message}");
                                    LogMessage($"InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                                    LogMessage($"Waiting for service {this.RunTimeController?.JobName} tick in {timer!.Period}");
                                }
                            }
                        }
                        else
                        {
                            if (this.RunTimeController != null)
                            {
                                LogMessage($"Is Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                await SendAlertServiceRunning();
                                await SetRuntimeControllerAsync(false);
                                LogMessage($"Recovery Service {this.RunTimeController?.JobName} ---> Waiting for next execute 00:30:00");
                                await Task.Delay(500);
                            }
                            else
                            {
                                await SendAlertTimerIsNull();
                                LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                                hasNextExecute = false;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage(ex.Message);
                    }
                    await SetPeriodicTimer();
                    hasNextExecute = Timer != null;
                }
                await SetRuntimeControllerAsync(false);
                this.LogMessage($"Ended --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
            
        }

        #endregion

        #region RunTimeController Methods

        private void InitiParameterContingency()
        {
            TimesWithoutTransaction = RunTimeController?.TimesWithoutTransaction ?? 0;
        }
        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this.PreLoadedEntitiesService!.FirstOrDefaulRunTimeControllerAsync(x => x.TypeService == this.Work!.TypeService);
        }
        protected void EndTransactionsContingencySum(int? totalValidTransactions)
        {
            if (totalValidTransactions == 0)
                TimesWithoutTransaction += 1;
            else
                TimesWithoutTransaction = 0;
            RunTimeController!.TimesWithoutTransaction = TimesWithoutTransaction;
            if (TimesWithoutTransaction > this.Options?.Value.MaxTimesWithoutTransactions)
            {
                if (!this.Options?.Value.InValidation ?? false)
                    RunTimeController!.IsContingecyTransaction = !RunTimeController!.IsContingecyTransaction;
                RunTimeController!.TimesWithoutTransaction = 0;
            }
        }
        private async Task SetRuntimeControllerAsync(bool isRunning)
        {
            RunTimeController!.IsRunning = isRunning;
            var response = await this.Mediator.Send(new RunTimeControllerUpdateCommand { Entity = RunTimeController });
            RunTimeController = response?.Entity;
        }
        private async Task SetPeriodicTimer()
        {
            if (RunTimeController == null)
            {
                RunTimeController = await GetRunTimeControllerAsync();
                InitiParameterContingency();
            }
            var minutesForTimeSpan = RunTimeController?.ConfigurationTimer ?? (decimal)1.00;
            Timer = new PeriodicTimer(TimeSpan.FromMinutes((double)minutesForTimeSpan));
        }

        #endregion

        #region Send Alerts For Log

        private async Task SendAlertExecute(PeriodicTimer timer)
        {
            await this.Mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = timer.Period
                }}),
                TypeOperationId = TypeOperationLogExecute?.ID
            });
        }
        private async Task SendAlertServiceRunning()
        {
            RunTimeController = await GetRunTimeControllerAsync();
            await this.Mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeOperationId = TypeOperationLogRunning?.ID
            });
        }
        private async Task SendAlertTimerIsNull()
        {
            await this.Mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeOperationId = TypeOperationLogLostConfiguration?.ID
            });
        }
        private async Task SendAlertServiceError(Exception ex, PeriodicTimer timer)
        {
            await this.Mediator.Send(new SendAlertMessageCommand
            {
                Parameters = SendAlertMessageCommand.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = timer.Period,
                    Exception = ex,
                }}),
                TypeOperationId = TypeOperationLogError?.ID
            });
        }

        #endregion

        #region Console Log

        private void LogMessage(string message)
        {
            switch (RunTimeController?.TypeService)
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

        #endregion
    }
}
