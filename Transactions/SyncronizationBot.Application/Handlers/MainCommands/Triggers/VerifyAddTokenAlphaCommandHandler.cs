using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.Triggers;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.Triggers
{
    public class VerifyAddTokenAlphaCommandHandler : IRequestHandler<VerifyAddTokenAlphaCommand, VerifyAddTokenAlphaCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenAlphaRepository _tokenAlphaRepository;
        private readonly ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private readonly ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        public VerifyAddTokenAlphaCommandHandler(IMediator mediator, 
                                                 ITokenAlphaRepository tokenAlphaRepository,
                                                 ITokenAlphaConfigurationRepository tokenAlphaConfigurationRepository,
                                                 ITokenAlphaWalletRepository tokenAlphaWalletRepository)
        {
            this._mediator = mediator;
            this._tokenAlphaRepository = tokenAlphaRepository;
            this._tokenAlphaConfigurationRepository = tokenAlphaConfigurationRepository;
            this._tokenAlphaWalletRepository = tokenAlphaWalletRepository;
        }
        public async Task<VerifyAddTokenAlphaCommandResponse> Handle(VerifyAddTokenAlphaCommand request, CancellationToken cancellationToken)
        {
            var tokenAlphaCalled = await this._tokenAlphaRepository.FindFirstOrDefault(x => x.TokenId == request.TokenId);
            if (tokenAlphaCalled != null)
            {
                var tokenAlphaBuyBefore = await this._tokenAlphaWalletRepository.FindFirstOrDefault(x => x.WalletId == request.WalletId);
                if (tokenAlphaBuyBefore != null)
                {
                    tokenAlphaBuyBefore.ValueSpentSol += request?.ValueBuySol;
                    tokenAlphaBuyBefore.ValueSpentUSDC += request?.ValueBuyUSDC;
                    tokenAlphaBuyBefore.ValueSpentUSDT += request?.ValueBuyUSDT;
                    tokenAlphaBuyBefore.NumberOfBuys += 1;
                    await this._tokenAlphaWalletRepository.Edit(tokenAlphaBuyBefore);
                    await this._tokenAlphaWalletRepository.DetachedItem(tokenAlphaBuyBefore);
                }
                else 
                {
                    var tokekAlphaWallet = await this._tokenAlphaWalletRepository.Add(new TokenAlphaWallet 
                    {
                        TokenAlphaId = tokenAlphaCalled.ID,
                        WalletId = request.WalletId,
                        NumberOfBuys = 1,
                        ValueSpentSol = request?.ValueBuySol,
                        ValueSpentUSDC = request?.ValueBuyUSDC,
                        ValueSpentUSDT = request?.ValueBuyUSDT
                    });
                    await this._tokenAlphaWalletRepository.DetachedItem(tokekAlphaWallet);
                }
                tokenAlphaCalled.IsCalledInChannel = false;
                await this._tokenAlphaRepository.Edit(tokenAlphaCalled);
                await this._tokenAlphaRepository.DetachedItem(tokenAlphaCalled);
            }
            else 
            { 
                //Verify if is a new token alpha and mapping
            }
            return new VerifyAddTokenAlphaCommandResponse { };
        }
    }
}
