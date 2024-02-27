using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Response;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Command
{
    public class ExecuteSendTelegramMessageCommand : IRequest<ExecuteSendTelegramMessageCommandResponse>
    {
        public long ChannelId { get; set; } = long.MinValue;
        public string? Message { get; set; } = string.Empty;
    }
}
