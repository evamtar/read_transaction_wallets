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
        private readonly ITokenRepository _tokenRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IClassWalletRepository _classWalletRepository;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        public SendAlertTokenAlphaCommandHandler(IMediator mediator,
                                                 ITokenRepository tokenRepository,
                                                 IWalletRepository walletRepository,
                                                 IClassWalletRepository classWalletRepository,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository)
        {
            this._mediator = mediator;
            this._tokenRepository = tokenRepository;
            this._walletRepository = walletRepository;
            this._classWalletRepository = classWalletRepository;
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
                var token = await this._tokenRepository.FindFirstOrDefault(x => x.ID == tokenAlpha!.TokenId);
                var tokensAlphaWalletsToAlert = await this._tokenAlphaWalletRepository.Get(x => x.TokenAlphaId == tokenAlpha!.ID);
                var listWalletsIds = this.GetListWalletsIds(tokensAlphaWalletsToAlert);
                var wallets = await this._walletRepository.Get(x => listWalletsIds.Contains(x.ID));
                var listClassWalletsIds = this.GetClassWalletsIds(wallets);
                var classWallets = await this._classWalletRepository.Get(x => listClassWalletsIds.Contains(x.ID));
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
                        token!,
                        tokensAlphaWalletsToAlert,
                        wallets,
                        classWallets!
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

        public int GetClassificationAlert(IEnumerable<TokenAlphaWallet> tokensAlphaWalletsToAlert) 
        {
            if (tokensAlphaWalletsToAlert.Count() > 4)
                return 4;
            return tokensAlphaWalletsToAlert.Count();
        }
        private List<Guid?> GetClassWalletsIds(IEnumerable<Wallet> wallets) 
        {
            var listIds = new List<Guid?>();
            foreach (var wallet in wallets)
                listIds.Add(wallet.ClassWalletId);
            return listIds;
        }

        private List<Guid?> GetListWalletsIds(IEnumerable<TokenAlphaWallet> tokenAlphaWallets) 
        {
            var listIds = new List<Guid?>();
            foreach (var wallets in tokenAlphaWallets)
                listIds.Add(wallets.WalletId);
            return listIds;
        }
    }
}
