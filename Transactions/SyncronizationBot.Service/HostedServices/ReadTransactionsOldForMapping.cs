

using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class ReadTransactionsOldForMapping : BaseService
    {
        public ReadTransactionsOldForMapping(IMediator mediator,
                                             IRunTimeControllerRepository runTimeControllerRepository,
                                             ITypeOperationRepository typeOperationRepository,
                                             IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.TransactionOldForMapping, syncronizationBotConfig)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ReadWalletsCommandForTransacionOldCommand { }, cancellationToken);
        }
    }
}
