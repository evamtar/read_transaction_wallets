using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Response.MainCommands.Delete;
using SyncronizationBot.Domain.Model.CrossCutting.Telegram.TelegramBot.Request;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Telegram;

namespace SyncronizationBot.Application.Handlers.MainCommands.Delete
{
    public class DeleteOldCallsCommandHandler : IRequestHandler<DeleteOldCallsCommand, DeleteOldCallsCommandResponse>
    {
        private IMediator _mediator;
        private readonly ITelegramMessageRepository _telegramMessageRepository;
        private readonly ITelegramChannelRepository _telegramChannelRepository;
        private readonly ITelegramBotService _telegramBotService;
        public DeleteOldCallsCommandHandler(IMediator mediator,
                                            ITelegramMessageRepository _telegramMessageRepository,
                                            ITelegramChannelRepository telegramChannelRepository,
                                            ITelegramBotService telegramBotService)
        {
            this._mediator = mediator;
            this._telegramMessageRepository = _telegramMessageRepository;
            this._telegramChannelRepository = telegramChannelRepository;
            this._telegramBotService = telegramBotService;
        }
        public async Task<DeleteOldCallsCommandResponse> Handle(DeleteOldCallsCommand request, CancellationToken cancellationToken)
        {
            var oldCallsMessage = await this._telegramMessageRepository.FindFirstOrDefault(x => x.EntityId == request.EntityId);
            var hasNext = oldCallsMessage != null;
            while (hasNext) 
            {
                var telegramChannel = await this._telegramChannelRepository.FindFirstOrDefault(x => x.ID == oldCallsMessage!.TelegramChannelId);
                var response = await this._telegramBotService.ExecuteDeleteMessagesAsync(new TelegramBotMessageDeleteRequest { MessageId = oldCallsMessage!.MessageId, ChatId = (long?)telegramChannel?.ChannelId });
                oldCallsMessage.IsDeleted = true;
                await this._telegramMessageRepository.Edit(oldCallsMessage);
                await this._telegramChannelRepository.DetachedItem(telegramChannel!);
                await this._telegramMessageRepository.DetachedItem(oldCallsMessage);
                oldCallsMessage = await this._telegramMessageRepository.FindFirstOrDefault(x => x.EntityId == request.EntityId);
                hasNext = oldCallsMessage != null;
            }
            return new DeleteOldCallsCommandResponse { };
        }
    }
}
