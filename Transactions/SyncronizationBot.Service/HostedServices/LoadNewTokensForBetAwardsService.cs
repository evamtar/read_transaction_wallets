using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Service.HostedServices.Base;


namespace SyncronizationBot.Service.HostedServices
{
    public class LoadNewTokensForBetAwardsService : BaseHostedService
    {
        protected override IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        protected override ETypeService? TypeService => ETypeService.NewTokensBetAwards;

        public LoadNewTokensForBetAwardsService(IMediator mediator) : base(mediator)
        {

        }

        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RecoverySaveNewsTokensCommand { }, cancellationToken);
        }



    }
}

