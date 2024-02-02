﻿using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class LoadBalanceWalletsService : BaseService
    {
        public LoadBalanceWalletsService(IMediator mediator,
                                         IRunTimeControllerRepository runTimeControllerRepository, 
                                         IOptions<SyncronizationBotConfig> syncronizationBotConfig) :base(mediator, runTimeControllerRepository, ETypeService.Balance, syncronizationBotConfig)
        {
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            base.LogMessage("Iniciando o serviço de atualização de saldo de conta");
            using var timer = await base.GetPeriodicTimer();
            if (timer != null)
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    base.LogMessage($"Init Balance Update: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                    if (base.RunTimeController != null && (!base.RunTimeController!.IsRunning ?? true))
                    {
                        try
                        {
                            await base.SetRuntimeControllerAsync(true, false);
                            await this._mediator.Send(new ReadWalletsBalanceCommand { });
                            await this._mediator.Send(new UpdateWalletsBalanceCommand{ });
                            await SetRuntimeControllerAsync(false, true);
                            base.LogMessage($"End Balance Update: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
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
                            base.LogMessage($"Waiting for next tick in {timer.Period}");
                        }
                    }
                    else
                    {
                        await base.SendAlertServiceRunning();
                        base.LogMessage($"Atualização de saldo Rodando: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
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
