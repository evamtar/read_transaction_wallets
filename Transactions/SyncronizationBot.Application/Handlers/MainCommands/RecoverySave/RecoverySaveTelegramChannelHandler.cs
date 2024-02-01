using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
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
            _mediator = mediator;
            _telegramBotService = telegramBotService;
            _telegramChannelRepository = telegramChannelRepository;
        }

        public async Task<RecoverySaveTelegramChannelResponse> Handle(RecoverySaveTelegramChannel request, CancellationToken cancellationToken)
        {
            var channel = await _telegramChannelRepository.FindFirstOrDefault(x => x.ID == request.TelegramChannelId);
            if (channel == null)
            {
                var channels = await _telegramBotService.ExecuteRecoveryChatAsync(new TelegramBotChatRequest { });
                var telegramChannel = channels.Result?.FirstOrDefault(x => x.ChatMember?.Chat?.Title == request.ChannelName);
                long? chatId = telegramChannel?.ChatMember?.Chat?.Id;
                channel = await _telegramChannelRepository.Add(new TelegramChannel
                {
                    ChannelName = request?.ChannelName,
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
