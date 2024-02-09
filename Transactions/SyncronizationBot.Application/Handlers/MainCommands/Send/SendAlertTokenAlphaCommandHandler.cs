﻿using MediatR;
using Newtonsoft.Json;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using System.Dynamic;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendAlertTokenAlphaCommandHandler : IRequestHandler<SendAlertTokenAlphaCommand, SendAlertTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IPublishMessageRepository _publishMessageRepository;
        
        public SendAlertTokenAlphaCommandHandler(IMediator mediator,
                                                 IPublishMessageRepository publishMessageRepository)
        {
            this._mediator = mediator;
            this._publishMessageRepository = publishMessageRepository;
        }

        public async Task<SendAlertTokenAlphaCommandResponse> Handle(SendAlertTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var publicMessage = await this._publishMessageRepository.FindFirstOrDefault(x => x.ItWasPublished == false && x.EntityParent == null);
            var hasNext = publicMessage != null;
            while (hasNext) 
            {
                var tokenAlpha = JsonConvert.DeserializeObject<TokenAlpha>(publicMessage!.JsonValue ?? string.Empty);
                var tokenAlphaConfiguration = await this.GetPublicMessage<TokenAlphaConfiguration>(publicMessage!.ID);
                var tokensAlphaWalletsToAlert = await this.GetListOfPublicMessage<TokenAlphaWallet>(publicMessage!.ID);
                //Limpar mensagens de calls anteriores do mesmo token
                await this._mediator.Send(new DeleteOldCallsCommand
                {
                    EntityId = publicMessage!.EntityId
                });
                await this._mediator.Send(new SendAlertMessageCommand
                {
                    IdClassification = this.GetClassificationAlert(tokensAlphaWalletsToAlert!),
                    EntityId = publicMessage!.EntityId,
                    Parameters = SendAlertMessageCommand.GetParameters(new object[]
                    {
                        tokenAlpha!,
                        tokenAlphaConfiguration!,
                        tokensAlphaWalletsToAlert
                    }),
                    TypeAlert = ETypeAlert.ALERT_TOKEN_ALPHA
                });
                publicMessage!.ItWasPublished = true;
                await this._publishMessageRepository.Edit(publicMessage!);
                await this._publishMessageRepository.DetachedItem(publicMessage!);
                publicMessage = await this._publishMessageRepository.FindFirstOrDefault(x => x.ItWasPublished == false && x.EntityParent == null);
                hasNext = publicMessage != null;
            }
            return new SendAlertTokenAlphaCommandResponse{ };
        }

        private async Task<List<T?>> GetListOfPublicMessage<T>(Guid? publicMessageParentId) where T : Entity 
        {
            var listOfEntity = new List<T?>();
            var listOfEntitiesIds = new List<Guid?>();
            var publicMessage = await this._publishMessageRepository.FindFirstOrDefault(x => x.EntityParentId == publicMessageParentId && x.ItWasPublished == false && x.Entity == typeof(T).ToString() && !listOfEntitiesIds.Contains(x.EntityId));
            var hasNext = publicMessage != null;
            while (hasNext) 
            {
                listOfEntitiesIds.Add(publicMessage!.EntityParentId);
                listOfEntity.Add(JsonConvert.DeserializeObject<T>(publicMessage?.JsonValue ?? string.Empty));
                publicMessage!.ItWasPublished = true;
                await this._publishMessageRepository.Edit(publicMessage);
                await this._publishMessageRepository.DetachedItem(publicMessage);
                publicMessage = await this._publishMessageRepository.FindFirstOrDefault(x => x.EntityParentId == publicMessageParentId && x.ItWasPublished == false && x.Entity == typeof(T).ToString() && !listOfEntitiesIds.Contains(x.EntityId));
                hasNext = publicMessage != null;
            }
            return listOfEntity;
        }

        private async Task<T?> GetPublicMessage<T>(Guid? publicMessageParentId) where T: Entity
        {
            var publicMessage = await this._publishMessageRepository.FindFirstOrDefault(x => x.EntityParentId == publicMessageParentId && x.ItWasPublished == false && x.Entity == typeof(T).ToString());
            if (publicMessage != null) 
            {
                publicMessage.ItWasPublished = true;
                await this._publishMessageRepository.Edit(publicMessage);
                await this._publishMessageRepository.DetachedItem(publicMessage);
                return JsonConvert.DeserializeObject<T>(publicMessage?.JsonValue ?? string.Empty);
            }
            return null!;
        }

        private int GetClassificationAlert(List<TokenAlphaWallet> tokensAlphaWalletsToAlert) 
        {
            if (tokensAlphaWalletsToAlert.Count() > 4)
                return 4;
            return tokensAlphaWalletsToAlert.Count();
        }
        
    }
}
