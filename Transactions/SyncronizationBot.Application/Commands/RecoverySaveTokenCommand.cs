using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class RecoverySaveTokenCommand : IRequest<RecoverySaveTokenCommandResponse>
    {
        public string? TokenHash { get; set; }
    }
}
