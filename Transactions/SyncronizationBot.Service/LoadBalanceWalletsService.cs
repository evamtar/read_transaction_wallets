using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;
using SyncronizationBot.Utils;


namespace SyncronizationBot.Service
{
    public class LoadBalanceWalletsService : BaseService
    {
        public LoadBalanceWalletsService(IMediator mediator,
                                         IRunTimeControllerRepository runTimeControllerRepository):base(mediator, runTimeControllerRepository)
        {
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            base.LogMessage("Iniciando o serviço de atualização de saldo de conta");
            using var timer = await base.GetPeriodicTimer(ETypeService.Balance);
            if (timer != null)
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    base.LogMessage($"Init Balance Update: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            await base.SetRuntimeControllerAsync(true, null, false);
                            await this._mediator.Send(new ReadWalletsBalanceCommand { });
                            await this._mediator.Send(new UpdateWalletsBalanceCommand{ });
                            await SetRuntimeControllerAsync(false, null, true);
                            base.LogMessage($"End Balance Update: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await base.SendAlertExecute(ETypeService.Balance, timer);
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                            base.LogMessage($"Final Ticks {DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now)}");
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
                        await base.SendAlertAppRunning(ETypeService.Balance, timer);
                        base.LogMessage($"Atualização de saldo Rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }

                }
            }
            else
            {
                await base.SendAlertTimerIsNull(ETypeService.Balance);
                base.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
            base.LogMessage("Finalizado");
            return;
        }
    }
}
