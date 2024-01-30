using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.SolanaFM.Base;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transactions.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using SyncronizationBot.Utils;
using System.Transactions;


namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionNotMappedRepository _transactionNotMappedRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository,
                                                      ITransactionNotMappedRepository transactionNotMappedRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig,
                                                      IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._transactionNotMappedRepository = transactionNotMappedRepository;
            this._mappedTokensConfig = mappedTokensConfig;
            this._syncronizationBotConfig = syncronizationBotConfig;
        }

        public async Task<RecoverySaveTransactionsCommandResponse> Handle(RecoverySaveTransactionsCommand request, CancellationToken cancellationToken)
        {
            var listTransactions = (List<TransactionsResponse>?)null!;
            if (request.IsContingecyTransactions ?? false)
            {
                var response = await _mediator.Send(new RecoveryTransactionsSignatureForAddressCommand
                {
                    WalletHash = request.WalletHash,
                    DateLoadBalance = request.DateLoadBalance,
                    InitialTicks = request.InitialTicks,
                    FinalTicks = request.FinalTicks
                });
                if (response != null)
                    listTransactions = response.Result;

            }
            else
            {
                var response = await _mediator.Send(new RecoveryTransactionsCommand
                {
                    WalletHash = request.WalletHash,
                    DateLoadBalance = request.DateLoadBalance,
                    InitialTicks = request.InitialTicks,
                    FinalTicks = request.FinalTicks
                });
                if (response != null)
                    listTransactions = response.Result;
            }
            if (listTransactions != null)
            {
                foreach (var transaction in listTransactions)
                {
                    var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = transaction.Signature });
                    if (transactionDetails?.Result != null && transactionDetails.Result.Data?.Count > 0)
                    {
                        try
                        {
                            var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
                            var transferAccount = TransferManagerHelper.GetTransferAccount(request.WalletHash, transactionDetails?.Result.Data[0].Source, transferManager);
                            var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, _mappedTokensConfig.Value);
                            if (transferInfo.TransactionType != ETransactionType.INDEFINED)
                            {
                                var tokenSended = (RecoverySaveTokenCommandResponse?)null;
                                var tokenSendedPool = (RecoverySaveTokenCommandResponse?)null;
                                var tokenReceived = (RecoverySaveTokenCommandResponse?)null;
                                var tokenReceivedPool = (RecoverySaveTokenCommandResponse?)null;
                                var tokenSolForPrice = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                                if (transferInfo?.TokenSended != null)
                                    tokenSended = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSended?.Token });
                                if (transferInfo?.TokenSendedPool != null)
                                    tokenSendedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSendedPool?.Token });
                                if (transferInfo?.TokenReceived != null)
                                    tokenReceived = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceived?.Token });
                                if (transferInfo?.TokenReceivedPool != null)
                                    tokenReceivedPool = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceivedPool?.Token });

                                var transactionDB = await _transactionsRepository.Add(new Transactions
                                {
                                    Signature = transaction?.Signature,
                                    DateOfTransaction = transaction?.DateOfTransaction,
                                    AmountValueSource = CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor),
                                    AmountValueSourcePool = CalculatedAmoutValue(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Divisor),
                                    AmountValueDestination = CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor),
                                    AmountValueDestinationPool = CalculatedAmoutValue(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Divisor),
                                    MtkcapTokenSource = CalculatedMarketcap(tokenSended?.MarketCap, tokenSended?.Supply, tokenSended?.Price),
                                    MtkcapTokenSourcePool = CalculatedMarketcap(tokenSendedPool?.MarketCap, tokenSendedPool?.Supply, tokenSendedPool?.Price),
                                    MtkcapTokenDestination = CalculatedMarketcap(tokenReceived?.MarketCap, tokenReceived?.Supply, tokenReceived?.Price),
                                    MtkcapTokenDestinationPool = CalculatedMarketcap(tokenReceivedPool?.MarketCap, tokenReceivedPool?.Supply, tokenReceivedPool?.Price),
                                    FeeTransaction = CalculatedFeeTransaction(transferInfo?.PaymentFee, tokenSolForPrice.Divisor),
                                    PriceTokenSourceUSD = tokenSended?.Price,
                                    PriceTokenSourcePoolUSD = tokenSendedPool?.Price,
                                    PriceTokenDestinationUSD = tokenReceived?.Price,
                                    PriceTokenDestinationPoolUSD = tokenReceivedPool?.Price,
                                    PriceSol = tokenSolForPrice.Price,
                                    TotalTokenSource = CalculatedTotalUSD(transferInfo?.TokenSended?.Amount, tokenSended?.Price, tokenSended?.Divisor),
                                    TotalTokenSourcePool = CalculatedTotalUSD(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Price, tokenSendedPool?.Divisor),
                                    TotalTokenDestination = CalculatedTotalUSD(transferInfo?.TokenReceived?.Amount, tokenReceived?.Price, tokenReceived?.Divisor),
                                    TotalTokenDestinationPool = CalculatedTotalUSD(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Price, tokenReceivedPool?.Divisor),
                                    IdTokenSource = tokenSended?.TokenId,
                                    IdTokenSourcePool = tokenSendedPool?.TokenId,
                                    IdTokenDestination = tokenReceived?.TokenId,
                                    IdTokenDestinationPool = tokenReceivedPool?.TokenId,
                                    IdWallet = request.WalletId,
                                    TypeOperation = (ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED)
                                });
                                var balancePosition = await UpdateBalance(transactionDB, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool);
                                await this._mediator.Send(new SendTransactionAlertsCommand 
                                {
                                    IdClassification = request.IdClassification,
                                    WalletId = request.WalletId,
                                    WalletHash = request.WalletHash,
                                    Transactions = transactionDB,
                                    DateOfTransfer = AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                                    TokenSendedSymbol = tokenSended?.Symbol,
                                    TokenSendedHash = tokenSended?.Hash,
                                    TokenSendedPoolSymbol = tokenSendedPool?.Symbol,
                                    TokenSendedPoolHash = tokenSendedPool?.Hash,
                                    TokenReceivedSymbol = tokenReceived?.Symbol,
                                    TokenReceivedHash = tokenReceived?.Hash,
                                    TokenReceivedName = tokenReceived?.Name,
                                    TokenReceivedMintAuthority = tokenReceived?.MintAuthority,
                                    TokenReceivedFreezeAuthority = tokenReceived?.FreezeAuthority,
                                    TokenReceivedIsMutable = tokenReceived?.IsMutable,
                                    TokenReceivedPoolSymbol = tokenReceivedPool?.Symbol,
                                    TokenReceivedPoolHash = tokenReceivedPool?.Hash,
                                    PercentModify = balancePosition.PercentModify,
                                    TokensMapped = this._mappedTokensConfig.Value.Tokens
                                });
                            }
                            else
                            {
                                await _transactionNotMappedRepository.Add(new TransactionNotMapped
                                {
                                    Signature = transaction.Signature,
                                    IdWallet = request.WalletId,
                                    Link = "https://solscan.io/tx/" + transaction.Signature,
                                    Error = ETransactionType.INDEFINED.ToString(),
                                    StackTrace = null,
                                    DateTimeRunner = DateTime.Now
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            await _transactionNotMappedRepository.Add(new TransactionNotMapped
                            {
                                Signature = transaction.Signature,
                                Link = "https://solscan.io/tx/" + transaction.Signature,
                                Error = ex.Message,
                                StackTrace = ex.StackTrace,
                                DateTimeRunner = DateTime.Now
                            });
                        }
                    }
                }
            }
            return new RecoverySaveTransactionsCommandResponse { };
        }

        private async Task<RecoveryAddUpdateBalanceItemCommandResponse> UpdateBalance(Transactions? transactions, TransferInfo? transferInfo, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool)
        {
            if (transferInfo?.PaymentFee.HasValue ?? false)
            {
                var solTokenForFee = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                {
                    WalleId = transactions?.IdWallet,
                    TokenId = solTokenForFee?.TokenId,
                    Signature = transactions?.Signature,
                    Quantity = transferInfo?.PaymentFee / (decimal?)solTokenForFee?.Divisor ?? 1,
                    TokenHash = solTokenForFee?.Hash,
                });
            }
            switch (transactions?.TypeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SWAP:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    return await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                case ETypeOperation.SELL:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                    return await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                case ETypeOperation.SEND:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    break;
                case ETypeOperation.RECEIVED:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                    break;
                case ETypeOperation.POOLCREATE:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSourcePool,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueSourcePool,
                        TokenHash = tokenSendedPool?.Hash,
                    });
                    break;
                case ETypeOperation.POOLFINALIZED:
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                    await _mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestinationPool,
                        Signature = transactions?.Signature,
                        Quantity = transactions?.AmountValueDestinationPool,
                        TokenHash = tokenReceivedPool?.Hash,
                    });
                    break;
                case ETypeOperation.NONE:
                case ETypeOperation.BURN:
                    break;
                default:
                    break;
            }
            return null!;
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
        private DateTime AdjustDateTimeToPtBR(DateTime? dateTime)
        {
            return dateTime?.AddHours(_syncronizationBotConfig.Value.GTMHoursAdjust ?? 0) ?? DateTime.MinValue;
        }
        private decimal? CalculatedTotalUSD(long? amount, decimal? price, int? divisor)
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
