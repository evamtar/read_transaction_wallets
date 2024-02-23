using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Response;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceRead.MultExternal.Price.Command
{
    public class ReadTokenPriceCommand : IRequest<ReadTokenPriceCommandResponse>
    {
        public string TokenHash { get; set; } = string.Empty;
    }
}
