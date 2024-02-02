using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendTelegramMessageCommandHandler : IRequestHandler<SendTelegramMessageCommand, SendTelegramMessageCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ITelegramMessageRepository _telegramMessageRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public SendTelegramMessageCommandHandler(IMediator mediator,
                                                 ITelegramBotService telegramBotService,
                                                 ITelegramMessageRepository telegramMessageRepository,
                                                 IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
            this._telegramMessageRepository = telegramMessageRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        public async Task<SendTelegramMessageCommandResponse> Handle(SendTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var channel = await _mediator.Send(new RecoverySaveTelegramChannel { TelegramChannelId = request.TelegramChannelId });
            var response = await _telegramBotService.ExecuteSendMessageAsync(new TelegramBotMessageSendRequest { ChatId = channel.ChannelId, Message = request.Message });
            await this._telegramMessageRepository.Add(new TelegramMessage 
            { 
                MessageId = response.Result?.MessageId ?? 0,
                DateSended = AdjustDateTimeToPtBR(response.Result?.DateOfMessage),
                TelegramChannelId = request.TelegramChannelId,
                IsDeleted = false
            });
            return new SendTelegramMessageCommandResponse { };
        }

        private DateTime? AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0);
        }
    }
}
