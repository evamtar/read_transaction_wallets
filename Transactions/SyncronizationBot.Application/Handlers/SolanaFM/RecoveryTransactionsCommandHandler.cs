﻿using MediatR;
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
    public class RecoveryTransactionsCommandHandler : IRequestHandler<RecoveryTransactionsCommand, RecoveryTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionsOldForMappingRepository _transactionsOldForMappingRepository;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;

        public RecoveryTransactionsCommandHandler(IMediator mediator, 
                                                  ITransactionsService transactionsService,
                                                  ITransactionsRepository transactionsRepository,
                                                  ITransactionsOldForMappingRepository transactionsOldForMappingRepository,
                                                  IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transactionsService = transactionsService;
            this._transactionsRepository = transactionsRepository;
            this._transactionsOldForMappingRepository = transactionsOldForMappingRepository;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        public async Task<RecoveryTransactionsCommandResponse> Handle(RecoveryTransactionsCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = new List<TransactionsResponse>();
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
                        var responseDataOrdened = transactionResponse.Result!.Data.OrderBy(x => x.BlockTime).ThenBy(x => x.DateOfTransaction);
                        foreach (var transaction in responseDataOrdened)
                        {
                            var exists = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                            if (exists == null)
                            {
                                if (request.DateLoadBalance < AdjustDateTimeToPtBR(transaction?.DateOfTransaction))
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
                                await this.SaveTransactionsOldForMapping(transaction);
                        }
                    }
                }
                page++;
                hasNextPage = transactionResponse.Result?.Pagination?.TotalPages > page;
            }
            return new RecoveryTransactionsCommandResponse { Result = listTransactions };
        }

        private async Task SaveTransactionsOldForMapping(Domain.Model.CrossCutting.Solanafm.Transactions.Response.TransactionResponse? transactions) 
        {
            var exists = this._transactionsOldForMappingRepository.FindFirstOrDefault(x => x.Signature == transactions!.Signature);
            if (exists == null)
            {
                await this._transactionsOldForMappingRepository.Add(new TransactionsOldForMapping 
                { 
                    Signature = transactions?.Signature,
                    DateOfTransaction = transactions?.DateOfTransaction,
                    CreateDate = DateTime.Now,
                    IdWallet = Guid.NewGuid(),
                    IsIntegrated = false,
                });
            }
        }

        private DateTime AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(this._syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue;
        }
    }
}
