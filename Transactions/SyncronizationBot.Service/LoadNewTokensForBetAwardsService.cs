﻿using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class LoadNewTokensForBetAwardsService : BaseService
    {
        public LoadNewTokensForBetAwardsService(IMediator mediator,
                                 IRunTimeControllerRepository runTimeControllerRepository,
                                 ITypeOperationRepository typeOperationRepository,
                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.NewTokensBetAwards, syncronizationBotConfig)
        {
            
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            var response = await this._mediator.Send(new RecoverySaveNewsTokensCommand { }, cancellationToken);
        }

        
        
    }
}

