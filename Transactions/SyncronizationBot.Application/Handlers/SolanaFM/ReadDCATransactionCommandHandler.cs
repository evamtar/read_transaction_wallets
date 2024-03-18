using MediatR;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Domain.Service.CrossCutting.SolanaRpc.Transactions;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class ReadDCATransactionCommandHandler : IRequestHandler<ReadDCATransactionCommand, ReadDCATransactionCommandResponse>
    {
        private Dictionary<string, string> DcaWallets = new Dictionary<string, string> { { "59E4E1EB-3CDD-4964-B284-F8977F25DF9F",  "DCAKxn5PFNN1mBREPWGdk1RXg5aVH9rPErLfBFEi2Emb" },
                                                                                         { "F4777529-2E8A-42E7-B9D0-AFB281C7F975",  "DCAKuApAuZtVNYLk3KTAVW9GLWVvPbnb5CxxRRmVgcTr" },
                                                                                         { "93898F95-0B0D-4612-BC11-67139B765AE7", "DCAK36VfExkPdAkYUQg6ewgxyinvcEyPLyHjRbmveKFw" }
        };
        private readonly IMediator _mediator;
        private readonly ISolanaTransactionService _solanaTransactionService;
        private readonly ITransactionsRPCRecoveryRepository _transactionsRPCRecoveryRepository;

        public ReadDCATransactionCommandHandler(IMediator mediator,
                                                ISolanaTransactionService solanaTransactionService,
                                                ITransactionsRPCRecoveryRepository transactionsRPCRecoveryRepository)
        {
            _mediator = mediator;
            _solanaTransactionService = solanaTransactionService;
            _transactionsRPCRecoveryRepository = transactionsRPCRecoveryRepository;
        }
        public async Task<ReadDCATransactionCommandResponse> Handle(ReadDCATransactionCommand request, CancellationToken cancellationToken)
        {
            foreach (var walletDCA in DcaWallets)
            {
                var transactionResponse = await _solanaTransactionService.ExecuteRecoveryTransactionsAsync(new TransactionRPCRequest
                {
                    WalletHash = walletDCA.Value
                });
                if (transactionResponse != null)
                {
                    if (transactionResponse.Count > 0)
                    {
                        int brokenCount = 0;
                        foreach (var transaction in transactionResponse)
                        {
                            if (brokenCount == 10)
                                break;
                            var exists = await _transactionsRPCRecoveryRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                            if (exists == null)
                            {
                                var transactionRPCAdded = await _transactionsRPCRecoveryRepository.Add(new TransactionsRPCRecovery
                                {
                                    ID = Guid.NewGuid(),
                                    Signature = transaction?.Signature,
                                    DateOfTransaction = transaction?.DateOfTransaction,
                                    BlockTime = transaction?.BlockTime,
                                    WalletId = Guid.Parse(walletDCA.Key),
                                    CreateDate = DateTime.Now,
                                    IsIntegrated = false,
                                    IsDCA = true,
                                });
                                await _transactionsRPCRecoveryRepository.DetachedItem(transactionRPCAdded);
                                brokenCount = 0;
                            }
                            else
                            {
                                brokenCount++;
                                await _transactionsRPCRecoveryRepository.DetachedItem(exists);
                            }
                        }
                    }
                }
                await Task.Delay(1000);
            }
            return new ReadDCATransactionCommandResponse { };
        }

    }
}
