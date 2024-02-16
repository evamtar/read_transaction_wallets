using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class AlertPriceService : BaseHostedService
    {
        protected override IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        protected override ETypeService? TypeService => ETypeService.Price;

        public AlertPriceService(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            await _mediator.Send(new SendAlertPriceCommand { }, cancellationToken);
        }

    }
}
