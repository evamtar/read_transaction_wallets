using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Response.MainCommands.Delete;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers.MainCommands.Delete
{
    public class DeleteTelegramMessageCommandHandler : IRequestHandler<DeleteTelegramMessageCommand, DeleteTelegramMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ITelegramChannelRepository _telegramChannelRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public DeleteTelegramMessageCommandHandler(IMediator mediator,
                                                   ITelegramBotService telegramBotService,
                                                   ITelegramChannelRepository telegramChannelRepository,
                                                  IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
            this._telegramChannelRepository = telegramChannelRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }
        public async Task<DeleteTelegramMessageCommandResponse> Handle(DeleteTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var telegramChannel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ChannelName == request.ChannelName);
            var messages = await this._telegramBotService.ExecuteRecoveryChannelUpdatesAsync(new TelegramBotChannelUpdateRequest { });
            var filteredUpdates = messages?.Result?.FindAll(x => x.ChannelPost?.MessageId != null && x.ChannelPost?.SenderChat?.Id == telegramChannel?.ChannelId);
            if (filteredUpdates?.Count > 0) 
            {
                foreach (var updates in filteredUpdates)
                {
                    var messageId = updates?.ChannelPost?.MessageId;
                    var dateOfMessage = AdjustDateTimeToPtBR(updates?.ChannelPost?.DateOfMessage);
                    if (DateTime.Now.AddHours(-1) > dateOfMessage) 
                        await this._telegramBotService.ExecuteDeleteMessagesAsync(new TelegramBotMessageDeleteRequest { MessageId = messageId, ChatId = (long?)telegramChannel?.ChannelId });
                }
            }
            return new DeleteTelegramMessageCommandResponse { };
        }

        private DateTime? AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0);
        }
    }
}
