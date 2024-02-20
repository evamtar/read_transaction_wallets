using MediatR;
using Newtonsoft.Json;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendAlertTokenAlphaCommandHandler : IRequestHandler<SendAlertTokenAlphaCommand, SendAlertTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        
        public SendAlertTokenAlphaCommandHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<SendAlertTokenAlphaCommandResponse> Handle(SendAlertTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var publicsMessages = new List<object>{ };
            if(publicsMessages?.Count > 0) 
            { 
                foreach(var publicMessagesdadsas in publicsMessages) 
                {
                    var publicMessage = new { ID = Guid.NewGuid(), JsonValue = string.Empty, EntityId = Guid.NewGuid() };
                    var tokenAlpha = JsonConvert.DeserializeObject<TokenAlpha>(publicMessage!.JsonValue ?? string.Empty);
                    var tokenAlphaConfiguration = await this.GetPublicMessage<TokenAlphaConfiguration>(publicMessage!.ID);
                    var tokensAlphaWalletsToAlert = await this.GetListOfPublicMessage<TokenAlphaWallet>(publicMessage!.ID);
                    //Limpar mensagens de calls anteriores do mesmo token
                    ///TODO-EVANDRO
                    //await this._mediator.Send(new DeleteOldCallsCommand
                    //{
                    //    EntityId = publicMessage!.EntityId
                    //});
                    await this._mediator.Send(new SendAlertMessageCommand
                    {
                        IdSubLevel = this.GetClassificationAlert(tokensAlphaWalletsToAlert!),
                        EntityId = publicMessage!.EntityId,
                        Parameters = SendAlertMessageCommand.GetParameters(new object[]
                        {
                        tokenAlpha!,
                        tokenAlphaConfiguration!,
                        tokensAlphaWalletsToAlert
                        }),
                        TypeOperationId = Guid.NewGuid()//ETypeAlert.ALERT_TOKEN_ALPHA
                    });
                    //publicMessage!.ItWasPublished = true;
                    //this._publishMessageRepository.Update(publicMessage!);
                    //await this._publishMessageRepository.DetachedItemAsync(publicMessage!);
                }
            }
            return new SendAlertTokenAlphaCommandResponse{ };
        }

        private async Task<List<T?>> GetListOfPublicMessage<T>(Guid? publicMessageParentId) where T : Entity 
        {
            var listOfEntity = new List<T?>();
            //var publicsMessages = await this._publishMessageRepository.GetAsync(x => x.EntityParentId == publicMessageParentId && x.ItWasPublished == false && x.Entity == typeof(T).ToString());
            //if (publicsMessages?.Count > 0) 
            //{
            //    foreach (var publicMessage in publicsMessages)
            //    {
            //        listOfEntity.Add(JsonConvert.DeserializeObject<T>(publicMessage?.JsonValue ?? string.Empty));
            //        publicMessage!.ItWasPublished = true;
            //        this._publishMessageRepository.Update(publicMessage);
            //        await this._publishMessageRepository.DetachedItemAsync(publicMessage);
            //    }
            //}
            return listOfEntity;
        }

        private async Task<T?> GetPublicMessage<T>(Guid? publicMessageParentId) where T: Entity
        {
            //var publicMessage = await this._publishMessageRepository.FindFirstOrDefaultAsync(x => x.EntityParentId == publicMessageParentId && x.ItWasPublished == false && x.Entity == typeof(T).ToString());
            //if (publicMessage != null) 
            //{
            //    publicMessage.ItWasPublished = true;
            //    this._publishMessageRepository.Update(publicMessage);
            //    await this._publishMessageRepository.DetachedItemAsync(publicMessage);
            //    return JsonConvert.DeserializeObject<T>(publicMessage?.JsonValue ?? string.Empty);
            //}
            return null!;
        }

        private int GetClassificationAlert(List<TokenAlphaWallet> tokensAlphaWalletsToAlert) 
        {
            if (tokensAlphaWalletsToAlert.Count() > 5)
                return 5;
            return tokensAlphaWalletsToAlert.Count();
        }
        
    }
}
