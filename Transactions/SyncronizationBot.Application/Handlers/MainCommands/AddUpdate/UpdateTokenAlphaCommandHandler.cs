using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
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
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private readonly ITokenAlphaWalletHistoryRepository _tokenAlphaWalletHistoryRepository;

        public UpdateTokenAlphaCommandHandler(IMediator mediator,
                                              ITokenAlphaRepository tokenAlphaRepository,
                                              ITokenAlphaHistoryRepository tokenAlphaHistoryRepository,
                                              ITokenAlphaWalletRepository tokenAlphaWalletRepository,
                                              ITokenAlphaWalletHistoryRepository tokenAlphaWalletHistoryRepository)
        {
            this._mediator = mediator;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaHistoryRepository = tokenAlphaHistoryRepository;
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
                    tokenAlphaWallet.NumberOfSells = tokenAlphaWallet.NumberOfSells ?? 0 + 1;
                    tokenAlphaWallet.ValueReceivedSol = tokenAlphaWallet.ValueReceivedSol ?? 0 + request?.AmountTokenSol;
                    tokenAlphaWallet.ValueReceivedUSDC = tokenAlphaWallet.ValueReceivedUSDC ?? 0 + request?.AmountTokenUSDC;
                    tokenAlphaWallet.ValueReceivedUSDT = tokenAlphaWallet.ValueReceivedUSDT ?? 0 + request?.AmountTokenUSDT;
                    tokenAlphaWallet.QuantityTokenSell = tokenAlphaWallet.QuantityTokenSell?? 0 + request?.AmountTokenSell;
                    await this._tokenAlphaWalletRepository.Edit(tokenAlphaWallet);
                    await this._tokenAlphaWalletRepository.DetachedItem(tokenAlphaWallet);
                    await SaveTokenAlphaWalletsHistory(request, tokenAlphaWallet);

                }
                tokenAlpha.ActualMarketcap = request?.MarketCap;
                tokenAlpha.ActualPrice = request?.Price;
                tokenAlpha.LastUpdate = DateTime.Now;
                await SaveTokenAlphaHistory(request, tokenAlpha);
                await this._tokenAlphaRepository.Edit(tokenAlpha!);
                await this._tokenAlphaRepository.DetachedItem(tokenAlpha!);
            }
            return new UpdateTokenAlphaCommandResponse { };
        }

        private async Task SaveTokenAlphaHistory(UpdateTokenAlphaCommand? request, TokenAlpha tokenAlpha)
        {
            var tokenAlphaHistory = await this._tokenAlphaHistoryRepository.Add(new TokenAlphaHistory
            {
                TokenAlphaId = tokenAlpha?.ID,
                CallNumber = tokenAlpha?.CallNumber,
                InitialMarketcap = tokenAlpha?.InitialMarketcap,
                ActualMarketcap = tokenAlpha?.ActualMarketcap,
                InitialPrice = tokenAlpha?.InitialPrice,
                ActualPrice = tokenAlpha?.ActualPrice,
                RequestMarketCap = request?.MarketCap,
                RequestPrice = request?.Price,
                CreateDate = tokenAlpha?.CreateDate,
                LastUpdate = tokenAlpha?.LastUpdate,
                TokenId = tokenAlpha?.TokenId,
                TokenHash = tokenAlpha?.TokenHash,
                TokenName = tokenAlpha?.TokenName,
                TokenSymbol = tokenAlpha?.TokenSymbol,
                TokenAlphaConfigurationId = tokenAlpha?.TokenAlphaConfigurationId
            });
            await this._tokenAlphaHistoryRepository.DetachedItem(tokenAlphaHistory);
        }

        private async Task SaveTokenAlphaWalletsHistory(UpdateTokenAlphaCommand? request, TokenAlphaWallet? tokenAlphaWallet)
        {
            var tokenAlphaWalletHistory = await this._tokenAlphaWalletHistoryRepository.Add(new TokenAlphaWalletHistory
            {
                TokenAlphaWalletId = tokenAlphaWallet?.ID,
                TokenAlphaId = tokenAlphaWallet?.TokenAlphaId,
                WalletId = tokenAlphaWallet?.WalletId,
                WalletHash = tokenAlphaWallet?.WalletHash,
                ClassWalletDescription = tokenAlphaWallet?.ClassWalletDescription,
                NumberOfBuys = tokenAlphaWallet?.NumberOfBuys,
                ValueSpentSol = tokenAlphaWallet?.ValueSpentSol,
                ValueSpentUSDC = tokenAlphaWallet?.ValueSpentUSDC,
                ValueSpentUSDT = tokenAlphaWallet?.ValueSpentUSDT,
                QuantityToken = tokenAlphaWallet?.QuantityToken,
                NumberOfSells = tokenAlphaWallet?.NumberOfSells,
                ValueReceivedSol = tokenAlphaWallet?.ValueReceivedSol,
                ValueReceivedUSDC = tokenAlphaWallet?.ValueReceivedUSDC,
                ValueReceivedUSDT = tokenAlphaWallet?.ValueReceivedUSDT,
                QuantityTokenSell = tokenAlphaWallet?.QuantityTokenSell,
                RequestValueInSol = request?.AmountTokenSol,
                RequestValueInUSDC = request?.AmountTokenUSDC,
                RequestValueInUSDT = request?.AmountTokenUSDC,
                RequestQuantityToken = request?.AmountTokenSell
            });
            await this._tokenAlphaWalletHistoryRepository.DetachedItem(tokenAlphaWalletHistory);
        }
    }
}
