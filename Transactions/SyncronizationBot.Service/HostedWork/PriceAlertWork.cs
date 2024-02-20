using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.HostedWork.Base;

namespace SyncronizationBot.Service.HostedWork
{
    public class PriceAlertWork : IHostWorkService
    {
        private readonly IMediator _mediator;
        
        public IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        public ETypeService? TypeService => throw new NotImplementedException();

        public PriceAlertWork(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task DoExecute(CancellationToken cancellationToken)
        {
            await _mediator.Send(new SendAlertPriceCommand { }, cancellationToken);
        }
    }
}
