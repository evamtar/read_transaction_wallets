using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Repository;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTokenCommandHandler : IRequestHandler<RecoverySaveTokenCommand, RecoverySaveTokenCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenRepository _tokenRepository;
        public RecoverySaveTokenCommandHandler(IMediator mediator, 
                                               ITokenRepository tokenRepository)
        {
            this._mediator = mediator;
            this._tokenRepository = tokenRepository;
        }
        public async Task<RecoverySaveTokenCommandResponse> Handle(RecoverySaveTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await this._tokenRepository.FindFirstOrDefault(x => x.Hash == request.TokenHash);
            if (token == null)
            {
                //RecoveryToken from solanaFM
                return new RecoverySaveTokenCommandResponse{};
            }
            else 
            {
                return new RecoverySaveTokenCommandResponse 
                { 
                    Decimals = token.Decimals,
                    TokenAlias = token.TokenAlias
                };
            }
        }
    }
}
