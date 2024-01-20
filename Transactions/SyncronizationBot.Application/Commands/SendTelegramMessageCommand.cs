using MediatR;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Enum;
using System.ComponentModel;

namespace SyncronizationBot.Application.Commands
{
    public class SendTelegramMessageCommand : IRequest<SendTelegramMessageCommandResponse>
    {
        public string? Message { get; set; }
        public ETelegramChannel Channel { get; set; }
    }
    
}
