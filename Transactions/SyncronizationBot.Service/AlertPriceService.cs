using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;

namespace SyncronizationBot.Service
{
    public class AlertPriceService : BaseService
    {
        public AlertPriceService(IMediator mediator, 
                                 IRunTimeControllerRepository runTimeControllerRepository) : base(mediator, runTimeControllerRepository)
        {
        
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            base.LogMessage("Iniciando o serviço de alerta de preços");
            using var timer = await base.GetPeriodicTimer(ETypeService.Price);
            if (timer != null)
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    base.LogMessage($"Init Alert Price: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            await base.SetRuntimeControllerAsync(true, null, false);
                            await this._mediator.Send(new SendAlertMessageCommand { });
                            await SetRuntimeControllerAsync(false, null, true);
                            base.LogMessage($"End Alert Price: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await base.SendAlertExecute(ETypeService.Price, timer);
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                        catch (Exception ex)
                        {
                            await this.DetachedRuntimeControllerAsync();
                            await SetRuntimeControllerAsync(false, null, true);
                            base.LogMessage($"Exceção: {ex.Message}");
                            base.LogMessage($"StackTrace: {ex.StackTrace}");
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                    }
                    else
                    {
                        await base.SendAlertAppRunning(ETypeService.Price, timer);
                        base.LogMessage($"Alerta de Preços Rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }

                }
            }
            else
            {
                await base.SendAlertTimerIsNull(ETypeService.Price);
                base.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
            base.LogMessage("Finalizado");
            return;
        }

        
    }
}
