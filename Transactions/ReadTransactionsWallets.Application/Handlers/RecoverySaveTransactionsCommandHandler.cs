using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
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
            var page = 1;
            var hasNextPage = true;
            while (hasNextPage) 
            {
                var transactionResponse = await this._transactionsService.ExecuteRecoveryTransactionsAsync(new TransactionsRequest
                {
                    Page = page,
                    UtcFrom = request.InitialTicks,
                    UtcTo = request.FinalTicks,
                    WalletPublicKey = request.WalletHash
                });
                //Todo new Code (Data COUNT = 0 NO TRANSACTIONS)
                page++;
                hasNextPage = transactionResponse.Result?.Pagination?.TotalPages > page;
            }
            return new RecoverySaveTransactionsCommandResponse { };
        }
    }
}
