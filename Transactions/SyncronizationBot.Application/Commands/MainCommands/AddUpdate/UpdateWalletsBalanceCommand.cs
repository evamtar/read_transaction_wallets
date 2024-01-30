using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands.MainCommands.AddUpdate
{
    public class UpdateWalletsBalanceCommand : IRequest<UpdateWalletsBalanceCommandResponse>
    {
    }
}
