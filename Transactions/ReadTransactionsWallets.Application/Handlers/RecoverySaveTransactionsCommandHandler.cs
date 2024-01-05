using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Request;
using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Model.Enum;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Domain.Service.CrossCutting;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITokenRepository tokenRepository,
                                                      ITransactionsService transactionsService,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository)
        {
            this._mediator = mediator;
            this._transactionsService = transactionsService;
            this._transfersService = transfersService;
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
                if (transactionResponse.Result != null) 
                {
                    if (transactionResponse.Result?.Data?.Count > 0)
                    {
                        foreach (var transaction in transactionResponse.Result!.Data)
                        {
                            var transactionDetails = await this._transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = transaction.Signature });
                            if (transactionDetails.Result != null) 
                            {
                                if (transactionDetails.Result.Data?.Count > 0)
                                {
                                    var transfers = transactionDetails.Result.Data.FindAll(x => x.Action == "transfer" && !string.IsNullOrEmpty(x.Token));
                                    if (transfers?.Count > 0) 
                                    {
                                        var transferFrom = transfers.First();
                                        var transferTo = transfers.Last();
                                        var operationType = ETypeOperation.Transfer;
                                        if (transferFrom.Token == transferTo.Token) { }  //RECEIVED TOKEN REWARD LIKE A BONKEARN
                                        else if (transferFrom.Source == request.WalletHash || transferTo.Destination == request.WalletHash || transferFrom.Token == "So11111111111111111111111111111111111111112")
                                            operationType = ETypeOperation.Buy;
                                        else if (transferFrom.Destination == request.WalletHash || 
                                                 transferTo.Source == request.WalletHash || 
                                                 transferTo.Token == "So11111111111111111111111111111111111111112")
                                            operationType = ETypeOperation.Sell;
                                        else
                                        {
                                            Console.WriteLine($" TX {transaction.Signature}");
                                        }
                                        if (operationType == ETypeOperation.Buy || operationType == ETypeOperation.Sell) 
                                        {
                                            var tokenFrom = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferFrom.Token });
                                            var tokenTO = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferTo.Token });
                                            var transction  = await this._transactionsRepository.Add(new Transactions 
                                            {
                                                Signature = transaction.Signature,
                                                DateOfTransaction = transaction.DateOfTransaction,
                                                AmountValueSource = transferFrom.Amount / tokenFrom.Divisor,
                                                AmountValueDestination = transferTo.Amount / tokenTO.Divisor,
                                                IdTokenSource = tokenFrom.TokenId,
                                                IdTokenDestination = tokenTO.TokenId,
                                                IdWallet = request.WalletId,
                                                TypeOperation = operationType,
                                                JsonResponse = null
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                page++;
                hasNextPage = transactionResponse.Result?.Pagination?.TotalPages > page;
            }
            return new RecoverySaveTransactionsCommandResponse { };
        }
    }
}
