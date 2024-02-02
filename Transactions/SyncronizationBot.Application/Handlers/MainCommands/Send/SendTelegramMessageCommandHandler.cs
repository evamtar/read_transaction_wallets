using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendTelegramMessageCommandHandler : IRequestHandler<SendTelegramMessageCommand, SendTelegramMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramBotService _telegramBotService;
        public SendTelegramMessageCommandHandler(IMediator mediator,
                                                 ITelegramBotService telegramBotService)
        {
            _mediator = mediator;
            _telegramBotService = telegramBotService;
        }

        public async Task<SendTelegramMessageCommandResponse> Handle(SendTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var channel = await _mediator.Send(new RecoverySaveTelegramChannel { TelegramChannelId = request.TelegramChannelId });
            var response = await _telegramBotService.ExecuteSendMessageAsync(new TelegramBotMessageSendRequest { ChatId = channel.ChannelId, Message = request.Message });
            return new SendTelegramMessageCommandResponse { };
        }
    }
}
