using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Response;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Info.Command
{
    public class ReadTokenInfoCommand : IRequest<ReadTokenInfoCommandResponse>
    {
        public string TokenHash { get; set; } = string.Empty;
    }
}
