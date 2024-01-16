using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
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
        public RunTimeController? RunTimeController { get; private set; }
        public BaseService(IMediator mediator,
                           IRunTimeControllerRepository runTimeControllerRepository)
        {
            this._mediator = mediator;
            this._runTimeControllerRepository = runTimeControllerRepository;
        }

        protected async Task<PeriodicTimer?> GetPeriodicTimer(ETypeService typeService) 
        { 
            if(this.RunTimeController == null) 
            { 
                this.RunTimeController = await this.GetRunTimeControllerAsync(typeService);
            }
            return new PeriodicTimer(TimeSpan.FromMinutes(this.RunTimeController?.ConfigurationTimer ?? 10));
        }

        protected async Task SetRuntimeControllerAsync(bool isRunning, decimal? unixTimeSeconds, bool detachedItem)
        {
            this.RunTimeController!.IsRunning = isRunning;
            this.RunTimeController!.UnixTimeSeconds = unixTimeSeconds;
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

        protected async Task SendAlertExecute(ETypeService typeService, PeriodicTimer timer) 
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_EXECUTE,
                                new object[] {
                                    EnumExtension.GetDescription(typeService) ?? string.Empty,
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                    timer.Period
                                })
            });
        }

        protected async Task SendAlertAppRunning(ETypeService typeService, PeriodicTimer timer)
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_APP_RUNNING,
                            new object[] {
                                EnumExtension.GetDescription(typeService) ?? string.Empty,
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                timer.Period
                            })
            });
        }

        protected async Task SendAlertTimerIsNull(ETypeService typeService)
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_APP_TIME_NULL,
                    new object[] {
                        EnumExtension.GetDescription(typeService) ?? string.Empty,
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    })
            });
        }


        protected void LogMessage(string message) 
        {
            Console.WriteLine(message);
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync(ETypeService typeService)
        {
            return await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false && x.TypeService == typeService);
        }

        
    }
}
