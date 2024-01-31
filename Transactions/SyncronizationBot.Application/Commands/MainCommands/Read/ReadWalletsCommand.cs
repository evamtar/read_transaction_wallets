using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Read;

namespace SyncronizationBot.Application.Commands.MainCommands.Read
{
    public class ReadWalletsCommand : IRequest<ReadWalletsCommandResponse>
    {
        public bool? IsContingecyTransactions { get; set; }
    }
}
