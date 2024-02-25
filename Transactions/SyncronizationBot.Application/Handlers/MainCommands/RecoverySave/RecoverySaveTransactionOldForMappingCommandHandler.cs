using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;



namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionOldForMappingCommandHandler : IRequestHandler<RecoverySaveTransactionOldForMappingCommand, RecoverySaveTransactionOldForMappingCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionNotMappedRepository _transactionNotMappedRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        public RecoverySaveTransactionOldForMappingCommandHandler(IMediator mediator,
                                                                   ITransfersService transfersService,
                                                                   IUnitOfWorkSqlServer unitOfWorkSqlServer,
                                                                   IOptions<MappedTokensConfig> mappedTokensConfig)
        {
            this._mediator = mediator;
            this._transfersService = transfersService;
            this._transactionsRepository = unitOfWorkSqlServer.TransactionsRepository;
            this._transactionNotMappedRepository = unitOfWorkSqlServer.TransactionNotMappedRepository;
            this._mappedTokensConfig = mappedTokensConfig;
        }

        public async Task<RecoverySaveTransactionOldForMappingCommandResponse> Handle(RecoverySaveTransactionOldForMappingCommand request, CancellationToken cancellationToken)
        {
            //var listTransactions = await this._transactionsOldForMappingRepository.GetAsync(x => x.WalletId == request.WalletId && x.IsIntegrated == false, x => x.DateOfTransaction!);
            //if (listTransactions != null)
            //{
            //    foreach (var transaction in listTransactions)
            //    {
            //        var exists = await this._transactionsRepository.FindFirstOrDefaultAsync(x => x.Signature == transaction.Signature);
            //        if (exists == null) 
            //        {
            //            var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = transaction.Signature });
            //            if (transactionDetails?.Result != null && transactionDetails.Result.Data?.Count > 0)
            //            {
            //                try
            //                {
            //                    var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
            //                    var transferAccount = TransferManagerHelper.GetTransferAccount(request?.WalletHash, transactionDetails?.Result.Data[0].Source, transferManager);
            //                    var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, _mappedTokensConfig.Value);
            //                    if (transferInfo.TransactionType != ETransactionType.INDEFINED)
            //                    {
            //                        var tokenSended = (RecoverySaveTokenCommandResponse?)null;
            //                        var tokenSendedPool = (RecoverySaveTokenCommandResponse?)null;
            //                        var tokenReceived = (RecoverySaveTokenCommandResponse?)null;
            //                        var tokenReceivedPool = (RecoverySaveTokenCommandResponse?)null;
            //                        var tokenSolForPrice = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
            //                        if (transferInfo?.TokenSended != null)
            //                            tokenSended = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSended?.Token });
            //                        if (transferInfo?.TokenSendedPool != null)
            //                            tokenSendedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSendedPool?.Token });
            //                        if (transferInfo?.TokenReceived != null)
            //                            tokenReceived = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceived?.Token });
            //                        if (transferInfo?.TokenReceivedPool != null)
            //                            tokenReceivedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceivedPool?.Token });

            //                        var transactionDB = await _transactionsRepository.AddAsync(new Transactions
            //                        {
            //                            Signature = transaction?.Signature,
            //                            DateTransactionUTC = transaction?.DateOfTransaction,
            //                            FeeTransaction = CalculatedFeeTransaction(transferInfo?.PaymentFee, tokenSolForPrice.Divisor),
            //                            PriceSol = tokenSolForPrice.Price,
            //                            TotalOperationSol = null, ///TODO:EVANDRO
            //                            WalletId = request?.WalletId,
            //                            WalletHash = request?.WalletHash,
            //                            ClassWallet = request?.ClassWallet?.Description,
            //                            TypeOperationId = null, ///TODO:EVANDRO
            //                            //TypeOperation = (ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED)
            //                        });
            //                        await this._transactionsRepository.DetachedItemAsync(transactionDB!);
            //                    }
            //                    else
            //                    {
            //                        var transactionNotMapped = await _transactionNotMappedRepository.AddAsync(new TransactionNotMapped
            //                        {
            //                            Signature = transaction.Signature,
            //                            WalletId = request?.WalletId,
            //                            Link = "https://solscan.io/tx/" + transaction.Signature,
            //                            Error = "INDEFINED",
            //                            StackTrace = null,
            //                            DateTimeRunner = DateTime.Now
            //                        });
            //                        await this._transactionNotMappedRepository.DetachedItemAsync(transactionNotMapped!);
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    await _transactionNotMappedRepository.AddAsync(new TransactionNotMapped
            //                    {
            //                        Signature = transaction.Signature,
            //                        WalletId = request?.WalletId,
            //                        Link = "https://solscan.io/tx/" + transaction.Signature,
            //                        Error = ex.Message,
            //                        StackTrace = ex.StackTrace,
            //                        DateTimeRunner = DateTime.Now
            //                    });
            //                }
            //            }
            //        }
            //        transaction!.IsIntegrated = true;
            //        this._transactionsOldForMappingRepository.Update(transaction);
            //        await this._transactionsOldForMappingRepository.DetachedItemAsync(transaction);
            //    }
            //}
            return new RecoverySaveTransactionOldForMappingCommandResponse { };
        }

        private decimal? CalculatedAmoutValue(decimal? value, int? divisor)
        {
            if (value == null || divisor == null) return null;
            return value / (divisor ?? 1) ?? 0;
        }
        private decimal? CalculatedMarketcap(decimal? marketCap, decimal? supply, decimal? price)
        {
            if (marketCap != null)
                return marketCap;
            else
                return supply * price;
        }
        private decimal? CalculatedTotal(long? amount, decimal? price, int? divisor)
        {
            var ajustedAmount = ((decimal?)amount ?? 0) / ((decimal?)divisor ?? 1);
            return ajustedAmount * price;
        }
        private decimal? CalculatedFeeTransaction(decimal? value, int? divisor)
        {
            if (value == null || divisor == null) return null;
            return value / (divisor ?? 1) ?? 0;
        }

    }
}
