using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Calculated;

namespace SyncronizationBot.Application.Commands.MainCommands.Calculated
{
    public class CalculatedProfitOperationCommand : IRequest<CalculatedProfitOperationCommandResponse>
    {
    }
}
