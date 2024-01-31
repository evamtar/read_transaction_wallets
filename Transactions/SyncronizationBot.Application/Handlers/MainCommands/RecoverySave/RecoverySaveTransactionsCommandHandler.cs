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
using SyncronizationBot.Domain.Model.Alerts;
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
        private readonly IClassWalletRepository _classWalletRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        private readonly IOptions<SyncronizationBotConfig> _syncronizationBotConfig;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository,
                                                      ITransactionNotMappedRepository transactionNotMappedRepository,
                                                      IClassWalletRepository classWalletRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig,
                                                      IOptions<SyncronizationBotConfig> syncronizationBotConfig)
        {
            this._mediator = mediator;
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._transactionNotMappedRepository = transactionNotMappedRepository;
            this._classWalletRepository = classWalletRepository;
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
                                    WalletHash = request?.WalletHash,
                                    ClassWallet = await this.GetClassificationDescription(request?.IdClassification),
                                    TypeOperation = (ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED)
                                });
                                var balancePosition = await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                                {
                                    Transactions = transactionDB,
                                    TokenSendedHash = tokenSended?.Hash,
                                    TokenSendedPoolHash = tokenSendedPool?.Hash,
                                    TokenReceivedHash = tokenReceived?.Hash,
                                    TokenReceivedPoolHash = tokenReceivedPool?.Hash
                                });
                                await this._mediator.Send(new SendTransactionAlertsCommand
                                {
                                    Parameters = TelegramMessageHelper.GetParameters(new object[]
                                                                                    {
                                                                                        transactionDB,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                                    }),
                                    IdClassification = request?.IdClassification,
                                    WalletId = request?.WalletId,
                                    WalletHash = request?.WalletHash,
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

        private async Task<string?> GetClassificationDescription(int? idClassification)
        {
            var classification = await this._classWalletRepository.FindFirstOrDefault(x => x.IdClassification == idClassification);
            if (classification != null)
                return classification.Description ?? string.Empty;
            return string.Empty;
        }

    }
}
