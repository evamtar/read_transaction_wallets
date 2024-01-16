using MediatR;
using SyncronizationBot.Application.Commands.Base;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class ReadWalletsCommand : SearchCommand, IRequest<ReadWalletsCommandResponse>
    {

    }
}
