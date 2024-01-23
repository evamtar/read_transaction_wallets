﻿using MediatR;
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
        protected readonly ETypeService _typeService;
        public RunTimeController? RunTimeController { get; private set; }
        public BaseService(IMediator mediator,
                           IRunTimeControllerRepository runTimeControllerRepository,
                           ETypeService typeService)
        {
            this._mediator = mediator;
            this._runTimeControllerRepository = runTimeControllerRepository;
            this._typeService = typeService;
        }

        protected async Task<PeriodicTimer?> GetPeriodicTimer() 
        { 
            if(this.RunTimeController == null) 
            { 
                this.RunTimeController = await this.GetRunTimeControllerAsync();
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
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_EXECUTE,
                                new object[] {
                                    EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                    timer.Period
                                })
            });
        }

        protected async Task SendAlertAppRunning()
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_APP_RUNNING,
                            new object[] {
                                EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                            })
            });
        }

        protected async Task SendAlertTimerIsNull()
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolanaLog,
                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.LOG_APP_TIME_NULL,
                    new object[] {
                        EnumExtension.GetDescription(this._typeService) ?? string.Empty,
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    })
            });
        }

        protected void LogMessage(string message) 
        {
            switch (this._typeService)
            {
                case ETypeService.Transaction:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ETypeService.Balance:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case ETypeService.Price:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    break;
            }
            Console.WriteLine(message);
        }

        private async Task<RunTimeController?> GetRunTimeControllerAsync()
        {
            return await this._runTimeControllerRepository.FindFirstOrDefault(x => x.IsRunning == false && x.TypeService == this._typeService);
        }

        
    }
}
