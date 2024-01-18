using MediatR;
using SyncronizationBot.Application.Response;

namespace SyncronizationBot.Application.Commands
{
    public class RecoveryPriceCommand : IRequest<RecoveryPriceCommandResponse>
    {
        public List<string>? Ids { get; set; }
    }
}
