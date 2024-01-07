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
            var channels = await this._telegramBotService.ExecuteRecoveryChatAsync(new TelegramBotRequest { });

            long? chatId = long.MinValue;
            switch (request.Channel)
            {
                case EChannel.CallSolanaLog:
                    var chatLog = channels.Result?.FirstOrDefault(x => x.ChatMember?.Chat?.Title == EChannel.CallSolanaLog.ToString());
                    chatId = chatLog?.ChatMember?.Chat?.Id;
                    break;
                case EChannel.CallSolana:
                    var chatCall = channels.Result?.FirstOrDefault(x => x.ChatMember?.Chat?.Title == EChannel.CallSolana.ToString());
                    chatId = chatCall?.ChatMember?.Chat?.Id;
                    break;
                default:
                    break;
            }
            await this._telegramBotService.ExecuteSendMessageAsync(new TelegramBotRequest { ChatId = chatId, Message = request.Message });
            return new SendTelegramMessageCommandResponse { };
        }
    }
}
