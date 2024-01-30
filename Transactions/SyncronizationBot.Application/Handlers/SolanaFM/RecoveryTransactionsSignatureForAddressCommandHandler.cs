using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM.Base;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoveryTransactionsSignatureForAddressCommandHandler : IRequestHandler<RecoveryTransactionsSignatureForAddressCommand, RecoveryTransactionsSignatureForAddressCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsSignatureForAddressService _transactionsSignatureForAddressService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;

        public RecoveryTransactionsSignatureForAddressCommandHandler(IMediator mediator,
                                                                     ITransactionsSignatureForAddressService transactionsSignatureForAddressService,
                                                                     ITransactionsRepository transactionsRepository,
                                                                     IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transactionsSignatureForAddressService = transactionsSignatureForAddressService;
            this._transactionsRepository = transactionsRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        public async Task<RecoveryTransactionsSignatureForAddressCommandResponse> Handle(RecoveryTransactionsSignatureForAddressCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = new List<TransactionsResponse>();
            var response = await this._transactionsSignatureForAddressService.ExecuteRecoveryTransactionsForAddressAsync(new TransactionsSignatureForAddressRequest { WalletPublicKeyHash = request?.WalletHash });
            if(response?.Result?.Count > 0) 
            {
                var responseDataOrdened = response.Result!.OrderBy(x => x.BlockTime).ThenBy(x => x.DateOfTransaction);
                foreach (var transaction in responseDataOrdened)
                {
                    var exists = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                    if (exists == null)
                    {
                        if (request?.DateLoadBalance < AdjustDateTimeToPtBR(transaction?.DateOfTransaction))
                        {
                            listTransactions.Add(new TransactionsResponse
                            {
                                Signature = transaction?.Signature,
                                BlockTime = transaction?.BlockTime
                            });
                        }
                    }
                }
            }
            return new RecoveryTransactionsSignatureForAddressCommandResponse { Result = listTransactions };
        }

        private DateTime AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue;
        }
    }
}
