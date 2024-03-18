using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoveryTransactionsCommandHandler : BaseTransactionsHandler, IRequestHandler<RecoveryTransactionsCommand, RecoveryTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsRPCRecoveryRepository _transactionsRPCRecoveryRepository;
        
        public RecoveryTransactionsCommandHandler(IMediator mediator,
                                                  ITransactionsService transactionsService,
                                                  ITransactionsRPCRecoveryRepository transactionsRPCRecoveryRepository,
                                                  ITransactionsOldForMappingRepository transactionsOldForMappingRepository,
                                                  IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(transactionsOldForMappingRepository, syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transactionsService = transactionsService;
            this._transactionsRPCRecoveryRepository = transactionsRPCRecoveryRepository;
        }

        public async Task<RecoveryTransactionsCommandResponse> Handle(RecoveryTransactionsCommand request, CancellationToken cancellationToken)
        {
            var page = 1;
            var hasNextPage = true;
            while (hasNextPage)
            {
                var transactionResponse = await this._transactionsService.ExecuteRecoveryTransactionsAsync(new TransactionsRequest
                {
                    WalletPublicKey = request?.WalletHash,
                    UtcFrom = request?.InitialTicks,
                    UtcTo = request?.FinalTicks,
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
                }
                page++;
                hasNextPage = !hasNextPage ? hasNextPage : transactionResponse?.Result?.Pagination?.TotalPages > page;
            }
            return new RecoveryTransactionsCommandResponse { };
        }
    }
}
