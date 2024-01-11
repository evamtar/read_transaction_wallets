using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Request;
using ReadTransactionsWallets.Domain.Service.CrossCutting;

namespace ReadTransactionsWallets.Application.Handlers
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
            await this._telegramBotService.ExecuteSendMessageAsync(new TelegramBotRequest { ChatId = channel.ChannelId, Message = request.Message });
            return new SendTelegramMessageCommandResponse { };
        }
    }
}
