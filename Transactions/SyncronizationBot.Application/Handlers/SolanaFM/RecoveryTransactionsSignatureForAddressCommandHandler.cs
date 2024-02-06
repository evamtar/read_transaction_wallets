using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM.Base;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoveryTransactionsSignatureForAddressCommandHandler : BaseTransactionsHandler, IRequestHandler<RecoveryTransactionsSignatureForAddressCommand, RecoveryTransactionsSignatureForAddressCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsSignatureForAddressService _transactionsSignatureForAddressService;
        private readonly ITransactionsRepository _transactionsRepository;
        

        public RecoveryTransactionsSignatureForAddressCommandHandler(IMediator mediator,
                                                                     ITransactionsSignatureForAddressService transactionsSignatureForAddressService,
                                                                     ITransactionsRepository transactionsRepository,
                                                                     ITransactionsOldForMappingRepository transactionsOldForMappingRepository,
                                                                     IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(transactionsOldForMappingRepository, syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transactionsSignatureForAddressService = transactionsSignatureForAddressService;
            this._transactionsRepository = transactionsRepository;
        }

        public async Task<RecoveryTransactionsSignatureForAddressCommandResponse> Handle(RecoveryTransactionsSignatureForAddressCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = new List<TransactionsResponse>();
            var response = await this._transactionsSignatureForAddressService.ExecuteRecoveryTransactionsForAddressAsync(new TransactionsSignatureForAddressRequest { WalletPublicKeyHash = request?.WalletHash });
            if (response?.Result?.Count > 0)
            {
                var responseDataOrdened = response.Result!.OrderBy(x => x.BlockTime).ThenBy(x => x.DateOfTransaction);
                foreach (var transaction in responseDataOrdened)
                {
                    var exists = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                    if (exists == null)
                    {
                        if (request?.DateLoadBalance < base.AdjustDateTimeToPtBR(transaction?.DateOfTransaction))
                        {
                            listTransactions.Add(new TransactionsResponse
                            {
                                Signature = transaction?.Signature,
                                BlockTime = transaction?.BlockTime
                            });
                        }
                        else
                            await this.SaveTransactionsOldForMapping(transaction);
                    }
                    else 
                    {
                        await this._transactionsRepository.DetachedItem(exists);
                        await this.SaveTransactionsOldForMapping(transaction);
                    }
                }
            }
            return new RecoveryTransactionsSignatureForAddressCommandResponse { Result = listTransactions };
        }

    }
}
