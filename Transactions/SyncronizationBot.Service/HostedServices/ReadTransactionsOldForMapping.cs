using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class ReadTransactionsOldForMapping : BaseHostedService
    {
        protected override IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        protected override ETypeService? TypeService => ETypeService.TransactionOldForMapping;

        public ReadTransactionsOldForMapping(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ReadWalletsCommandForTransacionOldCommand { }, cancellationToken);
        }
    }
}
