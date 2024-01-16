using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;
using SyncronizationBot.Utils;


namespace SyncronizationBot.Service
{
    public class ReadTransactionWalletsService: BaseService
    {
        public ReadTransactionWalletsService(IMediator mediator,
                                             IRunTimeControllerRepository runTimeControllerRepository) : base(mediator, runTimeControllerRepository)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            base.LogMessage("Iniciando o serviço de leitura de transações efetuadas nas wallets mapeadas");
            using var timer = await base.GetPeriodicTimer(ETypeService.Transaction);
            if (timer != null)
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    base.LogMessage($"Init Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    var initialTicks = base.RunTimeController?.UnixTimeSeconds ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now.AddDays(-1));
                    if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            var finalTicks = DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now);
                            await base.SetRuntimeControllerAsync(true, initialTicks, false);
                            base.LogMessage($"Initial Ticks: {initialTicks}");
                            base.LogMessage($"Final Ticks: {finalTicks}");
                            await this._mediator.Send(new ReadWalletsCommand { InitialTicks = initialTicks, FinalTicks = finalTicks });
                            await SetRuntimeControllerAsync(false, finalTicks, true);
                            base.LogMessage($"End Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await base.SendAlertExecute(ETypeService.Transaction, timer);
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                        catch (Exception ex)
                        {
                            await this.DetachedRuntimeControllerAsync();
                            await SetRuntimeControllerAsync(false, initialTicks, true);
                            base.LogMessage($"Exceção: {ex.Message}");
                            base.LogMessage($"StackTrace: {ex.StackTrace}");
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                    }
                    else
                    {
                        await base.SendAlertAppRunning(ETypeService.Transaction, timer);
                        base.LogMessage($"Aplicativo rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }

                }
            }
            else 
            {
                await base.SendAlertTimerIsNull(ETypeService.Transaction);
                base.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
            base.LogMessage("Finalizado");
            return;
        }
        
    }
}

