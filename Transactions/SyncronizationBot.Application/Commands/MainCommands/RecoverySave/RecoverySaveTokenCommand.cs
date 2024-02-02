using MediatR;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;

namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTokenCommand : IRequest<RecoverySaveTokenCommandResponse>
    {
        public string? TokenHash { get; set; }
    }
}
