using MediatR;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;

namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoveryPriceCommand : IRequest<RecoveryPriceCommandResponse>
    {
        public List<string>? Ids { get; set; }
    }
}
