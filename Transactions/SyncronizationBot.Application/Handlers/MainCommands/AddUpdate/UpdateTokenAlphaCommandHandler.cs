using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.AddUpdate
{
    public class UpdateTokenAlphaCommandHandler : IRequestHandler<UpdateTokenAlphaCommand, UpdateTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaHistoryRepository _tokenAlphaHistoryRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly ITokenAlphaWalletHistoryRepository _tokenAlphaWalletHistoryRepository;

        public UpdateTokenAlphaCommandHandler(IMediator mediator,
                                                 IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaHistoryRepository tokenAlphaHistoryRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                                 ITokenAlphaWalletHistoryRepository tokenAlphaWalletHistoryRepository)
        {
            this._mediator = mediator;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaHistoryRepository = tokenAlphaHistoryRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
            this._tokenAlphaWalletHistoryRepository = tokenAlphaWalletHistoryRepository;
        }
        public async Task<UpdateTokenAlphaCommandResponse> Handle(UpdateTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlpha = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId);
            if (tokenAlpha != null)
            {
                var tokenAlphaWallet = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.TokenAlphaId == tokenAlpha.ID && x.WalletId == request.WalletId);
                if (tokenAlphaWallet != null)
                {
                    //UpdateValues
                    tokenAlphaWallet.ValueSpentSol -= request?.AmountTokenSol;
                    tokenAlphaWallet.ValueSpentUSDC -= request?.AmountTokenUSDC;
                    tokenAlphaWallet.ValueSpentUSDT -= request?.AmountTokenUSDT;
                    tokenAlphaWallet.QuantityToken -= request?.AmountTokenSell;
                    await this._tokenAlphaWalletRepository.Edit(tokenAlphaWallet);
                    await this._tokenAlphaWalletRepository.DetachedItem(tokenAlphaWallet);

                }
                tokenAlpha.ActualMarketcap = request?.MarketCap;
                tokenAlpha.ActualPrice = request?.Price;
                tokenAlpha.LastUpdate = DateTime.Now;
                await this._tokenAlphaRepository.Edit(tokenAlpha!);
                await this._tokenAlphaRepository.DetachedItem(tokenAlpha!);
            }
            return new UpdateTokenAlphaCommandResponse { };
        }
    }
}
