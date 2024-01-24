using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class ReadTransactionWalletsService: BaseService
    {
        public ReadTransactionWalletsService(IMediator mediator,
                                             IRunTimeControllerRepository runTimeControllerRepository) : base(mediator, runTimeControllerRepository, ETypeService.Transaction)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            base.LogMessage("Iniciando o serviço de leitura de transações efetuadas nas wallets mapeadas");
            using var timer = await base.GetPeriodicTimer();
            if (timer != null)
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    base.LogMessage($"Init Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            await base.SetRuntimeControllerAsync(true, false);
                            await this._mediator.Send(new ReadWalletsCommand { });
                            await SetRuntimeControllerAsync(false, true);
                            base.LogMessage($"End Read: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                            await base.SendAlertExecute(timer);
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                        catch (Exception ex)
                        {
                            await this.DetachedRuntimeControllerAsync();
                            await SetRuntimeControllerAsync(false, true);
                            base.LogMessage($"Exceção: {ex.Message}");
                            base.LogMessage($"StackTrace: {ex.StackTrace}");
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                    }
                    else
                    {
                        await base.SendAlertAppRunning();
                        base.LogMessage($"Aplicativo rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                }
            }
            else 
            {
                await base.SendAlertTimerIsNull();
                base.LogMessage($"Timer está nulo ou não configurado: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            }
            base.LogMessage("Finalizado");
            return;
        }
        
    }
}

