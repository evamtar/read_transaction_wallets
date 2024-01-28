using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers
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
            if (request.Channel == ETelegramChannel.None) 
                channel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ID == request.TelegramChannelId);
            if (channel == null) 
            {
                var channels = await this._telegramBotService.ExecuteRecoveryChatAsync(new TelegramBotChatRequest { });
                var telegramChannel = channels.Result?.FirstOrDefault(x => x.ChatMember?.Chat?.Title == EnumExtension.GetDescription(request.Channel));
                long? chatId = telegramChannel?.ChatMember?.Chat?.Id;
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
