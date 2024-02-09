using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Delete;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using System.Dynamic;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendAlertTokenAlphaCommandHandler : IRequestHandler<SendAlertTokenAlphaCommand, SendAlertTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        public SendAlertTokenAlphaCommandHandler(IMediator mediator,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository)
        {
            this._mediator = mediator;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
        }

        public async Task<SendAlertTokenAlphaCommandResponse> Handle(SendAlertTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlpha = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.IsCalledInChannel == false);
            var hasNext = tokenAlpha != null;
            while (hasNext) 
            {
                var tokenAlphaConfiguration = await this._tokenAlphaConfigurationRepository.FindFirstOrDefault(x => x.ID == tokenAlpha!.TokenAlphaConfigurationId);
                var tokensAlphaWalletsToAlert = await this._tokenAlphaWalletRepository.Get(x => x.TokenAlphaId == tokenAlpha!.ID);
                //Limpar mensagens de calls anteriores do mesmo token
                await this._mediator.Send(new DeleteOldCallsCommand
                {
                    EntityId = tokenAlpha!.ID
                });
                await this._mediator.Send(new SendAlertMessageCommand
                {
                    IdClassification = this.GetClassificationAlert(tokensAlphaWalletsToAlert),
                    EntityId = tokenAlpha?.ID,
                    Parameters = SendAlertMessageCommand.GetParameters(new object[]
                    {
                        tokenAlpha!,
                        tokenAlphaConfiguration!,
                        tokensAlphaWalletsToAlert
                    }),
                    TypeAlert = ETypeAlert.ALERT_TOKEN_ALPHA
                });
                tokenAlpha!.IsCalledInChannel = true;
                tokenAlpha!.LastUpdate = DateTime.Now;
                await this._tokenAlphaRepository.Edit(tokenAlpha);
                await this._tokenAlphaRepository.DetachedItem(tokenAlpha);
                tokenAlpha = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.IsCalledInChannel == false);
                hasNext = tokenAlpha != null;
            }
            return new SendAlertTokenAlphaCommandResponse{ };
        }

        private int GetClassificationAlert(List<TokenAlphaWallet> tokensAlphaWalletsToAlert) 
        {
            if (tokensAlphaWalletsToAlert.Count() > 4)
                return 4;
            return tokensAlphaWalletsToAlert.Count();
        }
        
    }
}
