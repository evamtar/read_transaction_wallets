using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers
{
    public class SendTelegramMessageCommandHandler : IRequestHandler<SendTelegramMessageCommand, SendTelegramMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramBotService _telegramBotService;
        public SendTelegramMessageCommandHandler(IMediator mediator, 
                                                 ITelegramBotService telegramBotService)
        {
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
        }

        public async Task<SendTelegramMessageCommandResponse> Handle(SendTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var channel = await this._mediator.Send(new RecoverySaveTelegramChannel { Channel = request.Channel });
            var response = await this._telegramBotService.ExecuteSendMessageAsync(new TelegramBotMessageRequest { ChatId = channel.ChannelId, Message = request.Message });
            return new SendTelegramMessageCommandResponse { };
        }
    }
}
