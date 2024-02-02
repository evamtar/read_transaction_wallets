using MediatR;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;

namespace SyncronizationBot.Application.Commands.MainCommands.AddUpdate
{
    public class UpdateWalletsBalanceCommand : IRequest<UpdateWalletsBalanceCommandResponse>
    {
    }
}
