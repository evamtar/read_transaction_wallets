using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;
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
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
            this._telegramChannelRepository = telegramChannelRepository;
        }

        public async Task<RecoverySaveTelegramChannelResponse> Handle(RecoverySaveTelegramChannel request, CancellationToken cancellationToken)
        {
            var channel = await _telegramChannelRepository.FindFirstOrDefaultAsync(x => x.ID == request.TelegramChannelId);
            if (channel == null)
            {
                var channels = await _telegramBotService.ExecuteRecoveryChannelUpdatesAsync(new TelegramBotChannelUpdateRequest { });
                var telegramChannel = channels.Result?.FirstOrDefault(x => x.MyChatMember?.Chat?.Title == request.ChannelName);
                long? chatId = telegramChannel?.MyChatMember?.Chat?.Id;
                channel = await _telegramChannelRepository.AddAsync(new TelegramChannel
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
