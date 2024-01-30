using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Enum;
using System.ComponentModel;

namespace SyncronizationBot.Application.Commands.MainCommands.Send
{
    public class SendTelegramMessageCommand : IRequest<SendTelegramMessageCommandResponse>
    {
        public Guid? TelegramChannelId { get; set; }
        public string? Message { get; set; }
        public ETelegramChannel Channel { get; set; }
    }

}
