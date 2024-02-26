using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Response;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.TokenFull.Command
{
    public class ReadTokenFullInfoCommand : IRequest<ReadTokenFullInfoCommandResponse>
    {
        public string TokenHash { get; set; } = string.Empty;
    }
}
