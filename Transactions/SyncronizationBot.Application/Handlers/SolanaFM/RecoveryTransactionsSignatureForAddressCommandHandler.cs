﻿using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Application.Response.SolanaFM.Base;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;

namespace SyncronizationBot.Application.Handlers.SolanaFM
{
    public class RecoveryTransactionsSignatureForAddressCommandHandler : BaseTransactionsHandler, IRequestHandler<RecoveryTransactionsSignatureForAddressCommand, RecoveryTransactionsSignatureForAddressCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsSignatureForAddressService _transactionsSignatureForAddressService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionRPCRecoveryRepository _transactionsContingencyRepository;

        public RecoveryTransactionsSignatureForAddressCommandHandler(IMediator mediator,
                                                                     ITransactionsSignatureForAddressService transactionsSignatureForAddressService,
                                                                     ITransactionsRepository transactionsRepository,
                                                                     ITransactionRPCRecoveryRepository transactionsContingencyRepository,
                                                                     IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transactionsSignatureForAddressService = transactionsSignatureForAddressService;
            this._transactionsRepository = transactionsRepository;
            this._transactionsContingencyRepository = transactionsContingencyRepository;
        }

        public async Task<RecoveryTransactionsSignatureForAddressCommandResponse> Handle(RecoveryTransactionsSignatureForAddressCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = new List<TransactionsResponse>();
            var response = await this._transactionsSignatureForAddressService.ExecuteRecoveryTransactionsForAddressAsync(new TransactionsSignatureForAddressRequest { WalletPublicKeyHash = request?.WalletHash, Limit = request?.Limit });
            if (response?.Result?.Count > 0)
            {
                var responseDataOrdened = response.Result!.OrderBy(x => x.BlockTime).ThenBy(x => x.DateOfTransaction);
                foreach (var transaction in responseDataOrdened)
                {
                    var exists = await this._transactionsRepository.FindFirstOrDefaultAsync(x => x.Signature == transaction.Signature);
                    if (exists == null)
                    {
                        if (DateTime.Now.AddMinutes(-10) > base.AdjustDateTimeToPtBR(transaction?.DateOfTransaction))
                            await this.SaveTransactionsOldForMapping(transaction, request?.WalletId);
                        else 
                        {
                            var existsContingency = await this._transactionsContingencyRepository.FindFirstOrDefaultAsync(x => x.Signature == transaction.Signature);
                            if (existsContingency == null)
                                await this._transactionsContingencyRepository.AddAsync(new TransactionRPCRecovery 
                                {
                                    Signature = transaction?.Signature,
                                    DateOfTransaction = transaction?.DateOfTransaction,
                                    BlockTime  = transaction?.BlockTime,
                                    CreateDate = DateTime.Now,
                                    WalletId = request?.WalletId,
                                    IsIntegrated = false,
                                });
                        }
                    }
                    else 
                    {
                        await this.SaveTransactionsOldForMapping(transaction, request?.WalletId);
                    }
                }
                var listsTransactionsContingency = await this._transactionsContingencyRepository.GetAsync(x => x.WalletId == request.WalletId && x.DateOfTransaction < DateTime.Now.AddMinutes(-10) && x.IsIntegrated == false);
                listsTransactionsContingency.ForEach(delegate (TransactionRPCRecovery transaction) { listTransactions.Add(new TransactionsResponse 
                                                                                                                           { 
                                                                                                                               Signature = transaction.Signature, 
                                                                                                                               BlockTime = transaction.BlockTime 
                                                                                                                           }); 
                });
            }
            return new RecoveryTransactionsSignatureForAddressCommandResponse { Result = listTransactions };
        }

    }
}
