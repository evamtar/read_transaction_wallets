using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Calculated;
using SyncronizationBot.Application.Response.MainCommands.Calculated;

namespace SyncronizationBot.Application.Handlers.MainCommands.Calculated
{
    public class CalculatedProfitOperationCommandHandler : IRequestHandler<CalculatedProfitOperationCommand, CalculatedProfitOperationCommandResponse>
    {
        public CalculatedProfitOperationCommandHandler()
        {
            
        }

        public async Task<CalculatedProfitOperationCommandResponse> Handle(CalculatedProfitOperationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
