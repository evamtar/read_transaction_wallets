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
        private readonly ITelegramMessageRepository _telegramMessageRepository;
        private readonly ITelegramChannelRepository _telegramChannelRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public DeleteTelegramMessageCommandHandler(IMediator mediator,
                                                   ITelegramBotService telegramBotService,
                                                   ITelegramMessageRepository telegramMessageRepository,
                                                   ITelegramChannelRepository telegramChannelRepository,
                                                   IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._telegramBotService = telegramBotService;
            this._telegramMessageRepository = telegramMessageRepository;
            this._telegramChannelRepository = telegramChannelRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }
        public async Task<DeleteTelegramMessageCommandResponse> Handle(DeleteTelegramMessageCommand request, CancellationToken cancellationToken)
        {
            var dateTimeToDelete = this.GetDateOfSendedToDelete();
            var telegramChannel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ChannelName == request.ChannelName);
            var message = await this._telegramMessageRepository.FindFirstOrDefault(x => x.TelegramChannelId == telegramChannel!.ID && x.IsDeleted == false && x.DateSended < dateTimeToDelete, x => x.MessageId!);
            var hasNext = message != null;
            while (hasNext) 
            {
                var response = await this._telegramBotService.ExecuteDeleteMessagesAsync(new TelegramBotMessageDeleteRequest { MessageId = message.MessageId, ChatId = (long?)telegramChannel?.ChannelId });
                if (response.Result ?? false)
                {
                    message.IsDeleted = true;
                    await this._telegramMessageRepository.Edit(message);
                    await this._telegramMessageRepository.DetachedItem(message);
                }
                message = await this._telegramMessageRepository.FindFirstOrDefault(x => x.TelegramChannelId == telegramChannel!.ID && x.IsDeleted == false && x.DateSended < dateTimeToDelete, x => x.MessageId!);
                hasNext = message != null;
            }
            return new DeleteTelegramMessageCommandResponse { };
        }

        private DateTime? GetDateOfSendedToDelete()
        {
            return DateTime.Now.AddMinutes(this._syncronizationBotConfig.Value.MaxTimeBeforeDeleteLog ?? 0);
        }
    }
}
