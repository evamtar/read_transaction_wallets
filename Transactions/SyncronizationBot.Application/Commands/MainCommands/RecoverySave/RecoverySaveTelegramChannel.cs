using MediatR;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Application.Commands.MainCommands.RecoverySave
{
    public class RecoverySaveTelegramChannel : IRequest<RecoverySaveTelegramChannelResponse>
    {
        public Guid? TelegramChannelId { get; set; }
        public ETelegramChannel Channel { get; set; }
    }
}
