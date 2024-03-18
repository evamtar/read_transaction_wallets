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
            if (request?.ChannelsNames?.Count > 0) 
            {
                foreach (var channelName in request?.ChannelsNames!)
                {
                    var dateTimeToDelete = this.GetDateOfSendedToDelete(channelName);
                    var telegramChannel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ChannelName == channelName);
                    var messages = await this._telegramMessageRepository.Get(x => x.TelegramChannelId == telegramChannel!.ID && x.IsDeleted == false && x.DateSended < dateTimeToDelete, x => x.MessageId!);
                    if (messages?.Count > 0) 
                    {
                        foreach (var message in messages)
                        {
                            var response = await this._telegramBotService.ExecuteDeleteMessagesAsync(new TelegramBotMessageDeleteRequest { MessageId = message!.MessageId, ChatId = (long?)telegramChannel?.ChannelId });
                            if(response.Ok != null)
                               message.IsDeleted = true;
                            this._telegramMessageRepository.Edit(message);
                            await this._telegramMessageRepository.DetachedItem(message);
                        }
                        await this._telegramMessageRepository.SaveChangesASync();
                    }
                }
            }
            
            return new DeleteTelegramMessageCommandResponse { };
        }

        private DateTime? GetDateOfSendedToDelete(string? channelName)
        {
            switch (channelName)
            {
                case "CallSolana":
                case "AlertPriceChange":
                    return DateTime.Now.AddMinutes(this._syncronizationBotConfig.Value.MaxTimeBeforeTransactional ?? 0);
                case "CallSolanaLog":
                    return DateTime.Now.AddMinutes(this._syncronizationBotConfig.Value.MaxTimeBeforeDeleteLog ?? 0);
                case "TokenAlpha":
                case "TokenInfo":
                    return DateTime.Now.AddMinutes(this._syncronizationBotConfig.Value.MaxTimeBeforeAlpha ?? 0);
                default:
                    break;
            }
            return DateTime.Now.AddMinutes(this._syncronizationBotConfig.Value.MaxTimeBeforeDeleteLog ?? 0);
        }
    }
}
