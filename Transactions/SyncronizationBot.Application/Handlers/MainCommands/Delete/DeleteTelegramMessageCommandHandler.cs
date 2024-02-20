using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Response.MainCommands.Delete;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
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
            var telegramChannels = await this._telegramChannelRepository.GetAllAsync();
            if (telegramChannels?.Count > 0) 
            {
                foreach (var telegramChannel in telegramChannels)
                {
                    var dateTimeToDelete = this.GetDateOfSendedToDelete(telegramChannel);
                    var messages = await this._telegramMessageRepository.GetAsync(x => x.TelegramChannelId == telegramChannel!.ID && x.IsDeleted == false && x.DateSended < dateTimeToDelete, x => x.MessageId!);
                    if (messages?.Count > 0) 
                    {
                        foreach (var message in messages)
                        {
                            var response = await this._telegramBotService.ExecuteDeleteMessagesAsync(new TelegramBotMessageDeleteRequest { MessageId = message!.MessageId, ChatId = (long?)telegramChannel?.ChannelId });
                            if (response == null || response.Ok == null || !(response.Ok ?? false)) 
                            {
                                if(message.TryDeleted > 5)
                                   message.IsDeleted = true;
                                else
                                   message.TryDeleted = (message.TryDeleted ?? 0) + 1;
                            }
                            else
                                message.IsDeleted = true;
                            this._telegramMessageRepository.Update(message);
                            await this._telegramMessageRepository.DetachedItemAsync(message);
                        }
                    }
                }
            }
            
            return new DeleteTelegramMessageCommandResponse { };
        }

        private DateTime? GetDateOfSendedToDelete(TelegramChannel? channel)
        {
            return DateTime.Now.AddMinutes(channel?.TimeBeforeDelete ?? 20);
        }
    }
}
