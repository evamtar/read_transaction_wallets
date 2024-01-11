using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.TelegramBot.Request;
using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Domain.Service.CrossCutting;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTelegramChannelHandler : IRequestHandler<RecoverySaveTelegramChannel, RecoverySaveTelegramChannelResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ITelegramChannelRepository _telegramChannelRepository;
        public RecoverySaveTelegramChannelHandler(IMediator mediator,
                                                  ITelegramBotService telegramBotService,
                                                  ITelegramChannelRepository telegramChannelRepository)
        {
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
            this._telegramChannelRepository = telegramChannelRepository;
        }

        public async Task<RecoverySaveTelegramChannelResponse> Handle(RecoverySaveTelegramChannel request, CancellationToken cancellationToken)
        {
            var channel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ChannelName == request.Channel.ToString());
            if (channel == null) 
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
                channel = await this._telegramChannelRepository.Add(new TelegramChannel 
                { 
                    ChannelName = request.Channel.ToString(),
                    ChannelId = chatId,
                });
            }
            return new RecoverySaveTelegramChannelResponse
            {
                ID = channel.ID,
                ChannelId = (long?)channel.ChannelId,
                ChannelName = channel.ChannelName
            };
        }
    }
}
