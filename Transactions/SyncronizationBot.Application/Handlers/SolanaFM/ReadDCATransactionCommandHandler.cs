using MediatR;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
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
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsRPCRecoveryRepository _transactionsRPCRecoveryRepository;

        public ReadDCATransactionCommandHandler(IMediator mediator,
                                                  ITransactionsService transactionsService,
                                                  ITransactionsRPCRecoveryRepository transactionsRPCRecoveryRepository)
        {
            _mediator = mediator;
            _transactionsService = transactionsService;
            _transactionsRPCRecoveryRepository = transactionsRPCRecoveryRepository;
        }
        public async Task<ReadDCATransactionCommandResponse> Handle(ReadDCATransactionCommand request, CancellationToken cancellationToken)
        {
            var lastExecution = _transactionsRPCRecoveryRepository.GetMaxBlockTime(x => x.IsDCA == true) ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.UtcNow.AddMinutes(-8));
            var finalTicks = DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.UtcNow);
            foreach (var walletDCA in DcaWallets)
            {
                var page = 1;
                var hasNextPage = true;
                while (hasNextPage)
                {
                    var transactionResponse = await _transactionsService.ExecuteRecoveryTransactionsAsync(new TransactionsRequest
                    {
                        WalletPublicKey = walletDCA.Value,
                        UtcFrom = lastExecution,
                        UtcTo = finalTicks,
                        Page = page,
                    });
                    if (transactionResponse?.Result != null)
                    {
                        if (transactionResponse?.Result?.Data?.Count > 0)
                        {
                            int brokenCount = 0;
                            foreach (var transaction in transactionResponse.Result.Data)
                            {
                                if (transaction.Err == null)
                                {
                                    if (brokenCount == 10)
                                    {
                                        hasNextPage = false;
                                        break;
                                    }

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
                    }
                    page++;
                    hasNextPage = !hasNextPage ? hasNextPage : transactionResponse?.Result?.Pagination?.TotalPages > page;
                }
            }
            return new ReadDCATransactionCommandResponse { };
        }

    }
}
