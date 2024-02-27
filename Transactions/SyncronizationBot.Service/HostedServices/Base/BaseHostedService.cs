using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Alerts;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.RabbitMQ;
using SyncronizationBot.Domain.Service.HostedWork.Base;
using SyncronizationBot.Domain.Service.InternalService.Domains;
using SyncronizationBot.Domain.Service.InternalService.RunTime;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.LogMessageQueue;
using SyncronizationBot.Domain.Service.RabbitMQ.Queue.UpdateQueue;
using SyncronizationBot.Utils;
using System.Timers;


namespace SyncronizationBot.Service.HostedServices.Base
{
    public class BaseHostedService<T> : BackgroundService where T : IHostWorkService

    {
        
        #region Readonly Variables

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Properties
        protected System.Timers.Timer? Timer { get; set; }
        protected TimeSpan Interval { get; set; }
        protected CancellationToken CancellationToken { get; set; }
        protected T? Work { get; set; }
        protected ITypeOperationService TypeOperationService { get; private set; }
        protected IRunTimeControllerService RunTimeControllerService { get; private set; }
        protected IOptions<SyncronizationBotConfig>? Options { get; set; }
        protected RunTimeController? RunTimeController { get; set; }
        private IPublishLogService PublishLogService { get; set; }
        private IPublishUpdateService PublishUpdateService { get; set; }
        
        #endregion

        #region Constructors

        public BaseHostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.TypeOperationService = null!;
            this.RunTimeControllerService = null!;
            this.PublishLogService = null!;
            this.PublishUpdateService = null!;
            this.Options = null!;
            this.Timer = null!;
            this.Work = default;
        }

        #endregion

        #region ExecuteAsync Override

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.CancellationToken = stoppingToken;
            //Init Service
            await TrySetPeriodicTimer();
            Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Timer.Enabled = true;
            Timer.AutoReset = true;
            LogMessage($"***** Init Of Service --> ({typeof(T)}) --> Start in: {Interval} *****");
            Timer?.Start();
        }

        #endregion

        #region Events of Timer Elapse

        private async void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            TryStop();
            try
            {
                using (var scope = this._serviceProvider.CreateScope())
                {
                    this.InitScopedServices(scope);
                    await TrySetPeriodicTimer();
                    if (this.RunTimeController != null && (!this.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            await SetRuntimeControllerAsync(true);
                            LogMessage($"Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await this.Work!.DoExecute(this.CancellationToken);
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"End --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await SendAlertExecute();
                            LogMessage($"Waiting for service {this.RunTimeController?.JobName} next tick in {Interval}");
                            this.RunTimeControllerService.SaveChanges();
                            this.RunTimeController = null;
                            TryStart();
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                        catch (Exception ex)
                        {
                            await SendAlertServiceError(ex);
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"SERVICE -------------> : {this.RunTimeController?.JobName}");
                            LogMessage($"Exceção: {ex.Message}");
                            LogMessage($"StackTrace: {ex.StackTrace}");
                            LogMessage($"InnerException: {ex.InnerException}");
                            LogMessage($"InnerException---> Message: {ex.InnerException?.Message}");
                            LogMessage($"InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                            LogMessage($"Waiting for service {this.RunTimeController?.JobName} tick in {Interval}");
                            TryStart();
                            this.RunTimeController = null;
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                    }
                    else
                    {
                        if (this.RunTimeController != null)
                        {
                            LogMessage($"Is Running --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await SendAlertServiceRunning();
                            await SetRuntimeControllerAsync(false);
                            LogMessage($"Recovery Service {this.RunTimeController?.JobName} ---> Waiting for next execute {Interval}");
                            TryStart();
                            this.RunTimeController = null;
                            if (!CancellationToken.IsCancellationRequested) return;
                        }
                        else
                        {
                            await SendAlertTimerIsNull();
                            LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
            this.LogMessage($"Ended --> {this.RunTimeController?.JobName}: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
        }

        #endregion

        #region RunTimeController Methods

        private void InitScopedServices(IServiceScope scope) 
        {
            this.TypeOperationService = scope.ServiceProvider.GetRequiredService<ITypeOperationService>();
            this.RunTimeControllerService = scope.ServiceProvider.GetRequiredService<IRunTimeControllerService>();
            this.Work = scope.ServiceProvider.GetRequiredService<T>();
            this.PublishLogService = scope.ServiceProvider.GetRequiredService<IPublishLogService>();
            this.PublishUpdateService = scope.ServiceProvider.GetRequiredService<IPublishUpdateService>();
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this.RunTimeControllerService!.FindFirstOrDefaultAsync(x => x.TypeService == this.Work!.TypeService);
        }
        
        private async Task SetRuntimeControllerAsync(bool isRunning)
        {
            RunTimeController!.IsRunning = isRunning;
            this.RunTimeControllerService.Update(RunTimeController!);
            this.RunTimeControllerService.SaveChanges();
            await this.PublishUpdateService.Publish(new MessageEvent<RunTimeController>
            {
                CreateDate = DateTime.Now,
                Entity = this.RunTimeController,
                EventName = typeof(RunTimeController).Name + "_" + "UPDATE",
                Parameters = null
            });
        }
        private async Task TrySetPeriodicTimer()
        {
            if (this.RunTimeControllerService == null)
            {
                Interval = TimeSpan.FromMinutes((double)0.05);
                Timer = new System.Timers.Timer(Interval!);
                return;
            }
            if (RunTimeController == null)
            {
                RunTimeController = await GetRunTimeControllerAsync();
                var minutesForTimeSpan = RunTimeController?.ConfigurationTimer ?? (decimal)1.00;
                Interval = TimeSpan.FromMinutes((double)minutesForTimeSpan);
                Timer.Interval = Interval.TotalMilliseconds;
            }
        }

        #endregion

        #region Send Alerts For Log

        private async Task SendAlertExecute()
        {
            var typeOperation = await TypeOperationService.FindFirstOrDefaultAsync(x => x.IdTypeOperation == 0 && x.IdSubLevel == (int)ESubLeveTypeOperation.LogExecute);
            await this.PublishLogService.Publish(new MessageEvent<Entity>
            {
                CreateDate = DateTime.Now,
                EventName = Constants.LOG_EXECUTE,
                Parameters = Converters.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = Interval
                }}),
                TypeOperationId = typeOperation?.ID
            });
        }
        private async Task SendAlertServiceRunning()
        {
            var typeOperation = await TypeOperationService.FindFirstOrDefaultAsync(x => x.IdTypeOperation == 0 && x.IdSubLevel == (int)ESubLeveTypeOperation.LogAppRunning);
            await this.PublishLogService.Publish(new MessageEvent<Entity>
            {
                CreateDate = DateTime.Now,
                EventName = Constants.LOG_APP_RUNNING,
                Parameters = Converters.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeOperationId = typeOperation?.ID
            });
        }
        private async Task SendAlertTimerIsNull()
        {
            var typeOperation = await TypeOperationService.FindFirstOrDefaultAsync(x => x.IdTypeOperation == 0 && x.IdSubLevel == (int)ESubLeveTypeOperation.LogAppLostConfiguration);
            await this.PublishLogService.Publish(new MessageEvent<Entity>
            {
                CreateDate = DateTime.Now,
                EventName = Constants.LOG_LOST_CONFIGURATIO,
                Parameters = Converters.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now
                }}),
                TypeOperationId = typeOperation?.ID
            });
        }
        private async Task SendAlertServiceError(Exception ex)
        {
            var typeOperation = await TypeOperationService.FindFirstOrDefaultAsync(x => x.IdTypeOperation == 0 && x.IdSubLevel == (int)ESubLeveTypeOperation.LogError);
            await this.PublishLogService.Publish(new MessageEvent<Entity>
            {
                CreateDate = DateTime.Now,
                EventName = Constants.LOG_ERROR,
                Parameters = Converters.GetParameters(new object[] { new LogExecute
                {
                    ServiceName = RunTimeController?.JobName ?? string.Empty,
                    DateExecuted = DateTime.Now,
                    Timer = Interval,
                    Exception = ex,
                }}),
                TypeOperationId = typeOperation?.ID
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
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(message);
        }

        #endregion

        #region Disposable

        public override void Dispose()
        {
            try
            {
                TryStop();
                Timer?.Dispose();
                Work?.Dispose();
            }
            finally
            {

            }
            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void TryStop()
        {
            try
            {
                Timer?.Stop();
            }
            finally { }
        }

        private void TryStart()
        {
            try { if(!this.CancellationToken.IsCancellationRequested) Timer?.Start(); }
            finally { }
        }

        #endregion
    }
}
