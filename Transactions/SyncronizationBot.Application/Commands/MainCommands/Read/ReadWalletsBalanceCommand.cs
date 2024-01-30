using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands.MainCommands.Read
{
    public class ReadWalletsBalanceCommand : IRequest<ReadWalletsBalanceCommandResponse>
    {
    }
}
