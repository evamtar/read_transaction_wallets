using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;

namespace SyncronizationBot.Application.Handlers
{
    public class RecoveryPriceCommandHandler : IRequestHandler<RecoveryPriceCommand, RecoveryPriceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IJupiterPriceService _jupiterPriceService;
        public RecoveryPriceCommandHandler(IMediator mediator,
                                           IJupiterPriceService jupiterPriceService)
        {
            this._mediator = mediator;
            this._jupiterPriceService = jupiterPriceService;
        }
        public async Task<RecoveryPriceCommandResponse> Handle(RecoveryPriceCommand request, CancellationToken cancellationToken)
        {
            var prices = await this._jupiterPriceService.ExecuteRecoveryPriceAsync(new JupiterPricesRequest { Ids = request.Ids });
            return new RecoveryPriceCommandResponse
            {
                Data = prices.Data,
                TimeTaken = prices.TimeTaken
            };
        }
    }
}
