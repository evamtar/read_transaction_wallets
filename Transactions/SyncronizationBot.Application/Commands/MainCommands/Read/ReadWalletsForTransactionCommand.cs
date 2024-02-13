using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Read;

namespace SyncronizationBot.Application.Commands.MainCommands.Read
{
    public class ReadWalletsForTransactionCommand : IRequest<ReadWalletsForTransactionCommandResponse>
    {
        public bool? IsContingecyTransactions { get; set; }
    }
}
