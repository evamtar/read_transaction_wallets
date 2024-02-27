using MediatR;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Command;
using SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.ExternalServiceCommand.ExternalServiceExecute.Telegram.Message.Handler
{
    public class ExecuteSendTelegramMessageCommandHandler : IRequestHandler<ExecuteSendTelegramMessageCommand, ExecuteSendTelegramMessageCommandResponse>
    {
        private readonly ITelegramBotService _telegramBotService;
        public ExecuteSendTelegramMessageCommandHandler(ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }
        public async Task<ExecuteSendTelegramMessageCommandResponse> Handle(ExecuteSendTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var response = await _telegramBotService.ExecuteSendMessageAsync(new TelegramBotMessageSendRequest { ChatId = request?.ChannelId, Message = request?.Message });
            return new ExecuteSendTelegramMessageCommandResponse
            {
                MessageId = response.Result?.MessageId ?? 0,
                DateSended = response.Result?.DateOfMessage
            };
        }
    }
}
