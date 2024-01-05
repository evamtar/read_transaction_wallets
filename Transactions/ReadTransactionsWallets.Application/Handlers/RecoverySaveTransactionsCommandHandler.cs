using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Domain.Service.CrossCutting;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsRepository _transactionsRepository;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITokenRepository tokenRepository,
                                                      ITransactionsService transactionsService,
                                                      ITransactionsRepository transactionsRepository)
        {
            this._mediator = mediator;
            this._tokenRepository = tokenRepository;
            this._transactionsService = transactionsService;
            this._transactionsRepository = transactionsRepository;
        }

        public async Task<RecoverySaveTransactionsCommandResponse> Handle(RecoverySaveTransactionsCommand request, CancellationToken cancellationToken)
        {

            return new RecoverySaveTransactionsCommandResponse { };
        }
    }
}
