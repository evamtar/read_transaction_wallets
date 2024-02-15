using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Calculated;
using SyncronizationBot.Application.Response.MainCommands.Calculated;

namespace SyncronizationBot.Application.Handlers.MainCommands.Calculated
{
    public class CalculatedProfitCommandHandler : IRequestHandler<CalculatedProfitCommand, CalculatedProfitCommandResponse>
    {
        public CalculatedProfitCommandHandler()
        {
            
        }

        public async Task<CalculatedProfitCommandResponse> Handle(CalculatedProfitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
