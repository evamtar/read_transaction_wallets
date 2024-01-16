using MediatR;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Application.Commands
{
    public class RecoverySaveTelegramChannel : IRequest<RecoverySaveTelegramChannelResponse>
    {
        public ETelegramChannel Channel { get; set; }
    }
}
