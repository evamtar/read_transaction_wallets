using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.Base;


namespace SyncronizationBot.Service
{
    public class BalanceWalletsService : BaseServiceExecuteWithUpdate<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse, UpdateWalletsBalanceCommand, UpdateWalletsBalanceCommandResponse>
    {
        public BalanceWalletsService(IMediator mediator,
                                     IRunTimeControllerRepository runTimeControllerRepository,
                                     ITypeOperationRepository typeOperationRepository,
                                     IOptions<SyncronizationBotConfig> syncronizationBotConfig) :base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.Balance, syncronizationBotConfig)
        {
        }
    }
}
