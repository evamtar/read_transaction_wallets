using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;

namespace SyncronizationBot.Service
{
    public class AlertTokenAlphaService : BaseService
    {
        public AlertTokenAlphaService(IMediator mediator, 
                                      IRunTimeControllerRepository runTimeControllerRepository,
                                      IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, ETypeService.AlertTokenAlpha, syncronizationBotConfig)
        {
            base.LogMessage("Iniciando o serviço de alerta de token alpha");
        }

        protected override async Task DoExecute(PeriodicTimer timer, CancellationToken stoppingToken)
        {
            base.LogMessage($"Init Alert Token Alpha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
            {
                try
                {
                    await base.SetRuntimeControllerAsync(true, false);
                    await this._mediator.Send(new SendAlertTokenAlphaCommand { });
                    await SetRuntimeControllerAsync(false, true);
                    base.LogMessage($"End Token Alpha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    await base.SendAlertExecute(timer);
                    base.LogMessage($"Waiting for next tick in {timer.Period}");
                }
                catch (Exception ex)
                {
                    await base.SendAlertServiceError(ex, timer);
                    await this.DetachedRuntimeControllerAsync();
                    await SetRuntimeControllerAsync(false, true);
                    base.LogMessage($"Exceção: {ex.Message}");
                    base.LogMessage($"StackTrace: {ex.StackTrace}");
                    base.LogMessage($"InnerException: {ex.InnerException}");
                    base.LogMessage($"InnerException---> Message: {ex.InnerException?.Message}");
                    base.LogMessage($"InnerException--> StackTrace: {ex.InnerException?.StackTrace}");
                    base.LogMessage($"Waiting for next tick in {timer.Period}");
                }
            }
            else
            {
                await base.SendAlertServiceRunning();
                base.LogMessage($"Alerta de Token Alpha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
        }
        
    }
}
