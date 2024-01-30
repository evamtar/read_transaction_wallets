using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.CrossCutting.Jupiter.Prices.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Jupiter;

namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoveryPriceCommandHandler : IRequestHandler<RecoveryPriceCommand, RecoveryPriceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IJupiterPriceService _jupiterPriceService;
        public RecoveryPriceCommandHandler(IMediator mediator,
                                           IJupiterPriceService jupiterPriceService)
        {
            _mediator = mediator;
            _jupiterPriceService = jupiterPriceService;
        }
        public async Task<RecoveryPriceCommandResponse> Handle(RecoveryPriceCommand request, CancellationToken cancellationToken)
        {
            var prices = await _jupiterPriceService.ExecuteRecoveryPriceAsync(new JupiterPricesRequest { Ids = request.Ids });
            return new RecoveryPriceCommandResponse
            {
                Data = prices.Data,
                TimeTaken = prices.TimeTaken
            };
        }
    }
}
