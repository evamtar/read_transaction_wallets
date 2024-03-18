using MediatR;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using Solana.Unity.Wallet;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM.Base;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.SolanaRpc.Transactions;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoveryTransactionsCommandHandler : BaseTransactionsHandler, IRequestHandler<RecoveryTransactionsCommand, RecoveryTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ISolanaTransactionService _solanaTransactionService;
        private readonly ITransactionsRPCRecoveryRepository _transactionsRPCRecoveryRepository;
        
        public RecoveryTransactionsCommandHandler(IMediator mediator,
                                                  ISolanaTransactionService solanaTransactionService,
                                                  ITransactionsRPCRecoveryRepository transactionsRPCRecoveryRepository,
                                                  ITransactionsOldForMappingRepository transactionsOldForMappingRepository,
                                                  IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(transactionsOldForMappingRepository, syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._solanaTransactionService = solanaTransactionService;
            this._transactionsRPCRecoveryRepository = transactionsRPCRecoveryRepository;
        }

        public async Task<RecoveryTransactionsCommandResponse> Handle(RecoveryTransactionsCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = new List<TransactionsResponse>();
            var transactionResponse = await this._solanaTransactionService.ExecuteRecoveryTransactionsAsync(new TransactionRPCRequest
            {
                WalletHash = request?.WalletHash
            });
            if (transactionResponse != null && transactionResponse.Any())
            {
                if (transactionResponse.Count > 0)
                {
                    int brokenCount = 0;
                    foreach (var transaction in transactionResponse)
                    {
                        if (brokenCount == 20)
                            break;
                        var exists = await this._transactionsRPCRecoveryRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                        if (exists == null)
                        {
                            var transactionRPCAdded = await this._transactionsRPCRecoveryRepository.Add(new TransactionsRPCRecovery
                            {
                                ID = Guid.NewGuid(),
                                Signature = transaction?.Signature,
                                DateOfTransaction = transaction?.DateOfTransaction,
                                BlockTime = transaction?.BlockTime,
                                WalletId = request?.WalletId,
                                CreateDate = DateTime.Now,
                                IsIntegrated = false,
                            });
                            await this._transactionsRPCRecoveryRepository.DetachedItem(transactionRPCAdded);
                            brokenCount = 0;
                        }
                        else
                        {
                            brokenCount++;
                            await this._transactionsRPCRecoveryRepository.DetachedItem(exists);
                        }
                    }
                }
            }
            return new RecoveryTransactionsCommandResponse { Result = listTransactions };
        }
    }
}
