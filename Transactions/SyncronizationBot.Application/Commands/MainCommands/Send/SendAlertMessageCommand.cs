using MediatR;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Enum;

namespace SyncronizationBot.Application.Commands.MainCommands.Send
{
    public class SendAlertMessageCommand : IRequest<SendAlertMessageCommandResponse>
    {
        public ETypeAlert TypeAlert { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
    }
}
