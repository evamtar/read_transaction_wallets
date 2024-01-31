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
        protected readonly IMediator _mediator;
        protected readonly IRunTimeControllerRepository _runTimeControllerRepository;
        protected readonly ETypeService _typeService;
        protected readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        protected RunTimeController? RunTimeController { get; set; }
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
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        
        protected async Task<PeriodicTimer?> GetPeriodicTimer() 
        { 
            if(this.RunTimeController == null) 
            { 
                this.RunTimeController = await this.GetRunTimeControllerAsync();
                this.InitiParameterContingency();
            }
            return new PeriodicTimer(TimeSpan.FromMinutes(this.RunTimeController?.ConfigurationTimer ?? 1));
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
            try { if(this.RunTimeController != null) this.RunTimeController = await this._runTimeControllerRepository.DetachedItem(this.RunTimeController); }
            catch { }//NOTHING HERE 
            return this.RunTimeController;
        }

        protected async Task SendAlertExecute(PeriodicTimer timer)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                Parameters = TelegramMessageHelper.GetParameters(new object[] { new LogExecute
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
                Parameters = TelegramMessageHelper.GetParameters(new object[] { new LogExecute
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
                Parameters = TelegramMessageHelper.GetParameters(new object[] { new LogExecute
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
                Parameters = TelegramMessageHelper.GetParameters(new object[] { new LogExecute
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
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case ETypeService.Balance:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ETypeService.Price:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    break;
            }
            Console.WriteLine(message);
        }

        protected void EndTransactionsContingencySum(int? totalValidTransactions)
        {
            if (totalValidTransactions == 0)
                this.TimesWithoutTransactions = 0;
            else
                this.TimesWithoutTransactions += 1;
            if(this.TimesWithoutTransactions > this._syncronizationBotConfig.Value.MaxTimesWithoutTransactions)
                this.RunTimeController!.IsContingecyTransactions = !this.RunTimeController!.IsContingecyTransactions;
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false && x.TypeService == this._typeService);
        }

        private void InitiParameterContingency()
        {
            this.TimesWithoutTransactions = this.RunTimeController?.TimesWithoutTransactions ?? 0;
        }
    }
}
