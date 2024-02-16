using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Delete;

namespace SyncronizationBot.Application.Commands.MainCommands.Delete
{
    public class DeleteTelegramMessageCommand : IRequest<DeleteTelegramMessageCommandResponse>
    {
    }
}
