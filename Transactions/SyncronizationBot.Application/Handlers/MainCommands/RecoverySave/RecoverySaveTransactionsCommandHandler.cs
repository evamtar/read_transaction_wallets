using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Commands.MainCommands.Triggers;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.Transfers.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Helpers;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using System.Collections.Concurrent;


namespace SyncronizationBot.Application.Handlers.MainCommands.RecoverySave
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransfersService _transfersService;
        private readonly IWalletRepository _walletRepository;
        private readonly IClassWalletRepository _classWalletRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionsRPCRecoveryRepository _transactionsRPCRecoveryRepository;
        private readonly ITransactionNotMappedRepository _transactionNotMappedRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        private ConcurrentBag<ClassWallet> ClassWallets = new ConcurrentBag<ClassWallet>();
        private ConcurrentBag<Wallet> Wallets = new ConcurrentBag<Wallet>();
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITransfersService transfersService,
                                                      IWalletRepository walletRepository,
                                                      IClassWalletRepository classWalletRepository,
                                                      ITransactionsRepository transactionsRepository,
                                                      ITransactionsRPCRecoveryRepository transactionsRPCRecoveryRepository,
                                                      ITransactionNotMappedRepository transactionNotMappedRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig)
        {
            this._mediator = mediator;
            this._transfersService = transfersService;
            this._walletRepository = walletRepository;
            this._classWalletRepository = classWalletRepository;
            this._transactionsRepository = transactionsRepository;
            this._transactionsRPCRecoveryRepository = transactionsRPCRecoveryRepository;
            this._transactionNotMappedRepository = transactionNotMappedRepository;
            this._mappedTokensConfig = mappedTokensConfig;
        }

        
        public async Task<RecoverySaveTransactionsCommandResponse> Handle(RecoverySaveTransactionsCommand request, CancellationToken cancellationToken)
        {
            await this.LoadClassWallets();
            await this.LoadWallets();
            var listTransactions = await this._transactionsRPCRecoveryRepository.Get(x => x.IsIntegrated == false, x => x.DateOfTransaction!);
            if (listTransactions != null)
            {
                foreach (var transaction in listTransactions)
                {
                    var transactionDetails = await _transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = transaction.Signature });
                    if (transactionDetails?.Result != null && transactionDetails.Result.Data?.Count > 0)
                    {
                        try
                        {
                            var wallet = this.Wallets.FirstOrDefault(x => x.ID == transaction.WalletId);
                            var classWallet = this.ClassWallets.FirstOrDefault(x => x.ID == wallet?.ClassWalletId);

                            var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails?.Result?.Data);
                            var transferAccount = TransferManagerHelper.GetTransferAccount(wallet?.Hash, transactionDetails?.Result.Data[0].Source, transferManager);
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
                                    AmountValueSource = this.CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor),
                                    AmountValueSourcePool = this.CalculatedAmoutValue(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Divisor),
                                    AmountValueDestination = this.CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor),
                                    AmountValueDestinationPool = this.CalculatedAmoutValue(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Divisor),
                                    MtkcapTokenSource = this.CalculatedMarketcap(tokenSended?.MarketCap, tokenSended?.Supply, tokenSended?.Price),
                                    MtkcapTokenSourcePool = this.CalculatedMarketcap(tokenSendedPool?.MarketCap, tokenSendedPool?.Supply, tokenSendedPool?.Price),
                                    MtkcapTokenDestination = this.CalculatedMarketcap(tokenReceived?.MarketCap, tokenReceived?.Supply, tokenReceived?.Price),
                                    MtkcapTokenDestinationPool = this.CalculatedMarketcap(tokenReceivedPool?.MarketCap, tokenReceivedPool?.Supply, tokenReceivedPool?.Price),
                                    FeeTransaction = this.CalculatedFeeTransaction(transferInfo?.PaymentFee, tokenSolForPrice.Divisor),
                                    PriceTokenSourceUSD = tokenSended?.Price,
                                    PriceTokenSourcePoolUSD = tokenSendedPool?.Price,
                                    PriceTokenDestinationUSD = tokenReceived?.Price,
                                    PriceTokenDestinationPoolUSD = tokenReceivedPool?.Price,
                                    PriceSol = tokenSolForPrice.Price,
                                    TotalTokenSource = this.CalculatedTotal(transferInfo?.TokenSended?.Amount, tokenSended?.Price, tokenSended?.Divisor),
                                    TotalTokenSourcePool = this.CalculatedTotal(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Price, tokenSendedPool?.Divisor),
                                    TotalTokenDestination = this.CalculatedTotal(transferInfo?.TokenReceived?.Amount, tokenReceived?.Price, tokenReceived?.Divisor),
                                    TotalTokenDestinationPool = this.CalculatedTotal(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Price, tokenReceivedPool?.Divisor),
                                    TokenSourceId = tokenSended?.TokenId,
                                    TokenSourcePoolId = tokenSendedPool?.TokenId,
                                    TokenDestinationId = tokenReceived?.TokenId,
                                    TokenDestinationPoolId = tokenReceivedPool?.TokenId,
                                    WalletId = wallet?.ID,
                                    WalletHash = wallet?.Hash,
                                    ClassWallet = classWallet?.Description,
                                    TypeOperation = (ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED)
                                });
                                if (classWallet?.IdClassification == 6 && (transactionDB?.PriceTokenSourceUSD * transactionDB?.AmountValueSource) < 500)
                                    continue;
                                await this._transactionsRepository.DetachedItem(transactionDB!);
                                var balancePosition = await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                                {
                                    Transactions = transactionDB,
                                    TokenSendedHash = tokenSended?.Hash,
                                    TokenSendedPoolHash = tokenSendedPool?.Hash,
                                    TokenReceivedHash = tokenReceived?.Hash,
                                    TokenReceivedPoolHash = tokenReceivedPool?.Hash
                                });
                                if (transactionDB?.TypeOperation == ETypeOperation.BUY || transactionDB?.TypeOperation == ETypeOperation.SWAP)
                                {
                                    await this._mediator.Send(new VerifyAddTokenAlphaCommand
                                    {
                                        WalletId = wallet?.ID,
                                        WalletHash = wallet?.Hash,
                                        ClassWalletDescription = classWallet?.Description,
                                        TokenId = transactionDB?.TokenDestinationId,
                                        TokenHash = tokenReceived?.Hash,
                                        TokenName = tokenReceived?.Name,
                                        TokenSymbol = tokenReceived?.Symbol,
                                        ValueBuySol = this.CalculatedTotalSol(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        ValueBuyUSDC = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        ValueBuyUSDT = this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        QuantityTokenReceived = Math.Abs(transactionDB?.AmountValueDestination ?? 0),
                                        Signature = transactionDB?.Signature,
                                        MarketCap = transactionDB?.MtkcapTokenDestination ?? tokenReceived?.Supply * tokenReceived?.Price,
                                        Price = tokenReceived?.Price,
                                        LaunchDate = tokenReceived?.DateCreation ?? DateTime.Now,
                                    });
                                }
                                else if (transactionDB?.TypeOperation == ETypeOperation.SELL || transactionDB?.TypeOperation == ETypeOperation.SWAP) 
                                {
                                    await this._mediator.Send(new UpdateTokenAlphaCommand
                                    {
                                        WalletId = wallet?.ID,
                                        TokenId = transactionDB?.TokenSourceId,
                                        AmountTokenSol = this.CalculatedTotalSol(transferInfo?.TokenReceived?.Token, transactionDB?.AmountValueDestination, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        AmountTokenUSDC = this.CalculatedTotalUSD(transferInfo?.TokenReceived?.Token, transactionDB?.AmountValueDestination, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        AmountTokenUSDT = this.CalculatedTotalUSD(transferInfo?.TokenReceived?.Token, transactionDB?.AmountValueDestination, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation),
                                        AmountTokenSell = Math.Abs(transactionDB?.AmountValueSource ?? 0),
                                        MarketCap = transactionDB?.MtkcapTokenSource ?? tokenSended?.Supply * tokenSended?.Price,
                                        Price = tokenSended?.Price
                                    });
                                }
                                await this._mediator.Send(new SendTransactionAlertsCommand
                                {
                                    EntityId = transactionDB?.ID,
                                    Parameters = SendTransactionAlertsCommand.GetParameters(new object[]
                                                                                    {
                                                                                        transactionDB!,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                                    }),
                                    IdClassification = classWallet?.IdClassification,
                                    WalletId = wallet?.ID,
                                    Transactions = transactionDB,
                                    TokenSendedHash = tokenSended?.Hash,
                                    TokenReceivedHash = tokenReceived?.Hash,
                                    TokensMapped = this._mappedTokensConfig.Value.Tokens
                                });
                                if ((transactionDB?.TypeOperation == ETypeOperation.BUY || transactionDB?.TypeOperation == ETypeOperation.SWAP) && classWallet?.IdClassification == 7  && this.CalculatedTotalUSD(transferInfo?.TokenSended?.Token, transactionDB?.AmountValueSource, tokenSolForPrice.Price, tokenSended?.Price, transactionDB?.TypeOperation) > 9500)
                                {
                                    if (!this._mappedTokensConfig!.Value!.Tokens!.Contains(tokenSended!.Hash!) && !this._mappedTokensConfig!.Value!.Tokens!.Contains(tokenSended!.Hash!)) 
                                    {
                                        try
                                        {
                                            await this._mediator.Send(new SendAlertMessageCommand
                                            {
                                                EntityId = transactionDB?.ID,
                                                Parameters = SendTransactionAlertsCommand.GetParameters(new object[]
                                                                                        {
                                                                                        transactionDB!,
                                                                                        transferInfo!,
                                                                                        new List<RecoverySaveTokenCommandResponse?> { tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool } ,
                                                                                        balancePosition
                                                                                        }),
                                                TypeAlert = ETypeAlert.ALERT_WHALE_TRANSACTION

                                            });
                                        }
                                        catch
                                        {
                                            throw new Exception("TRANSACAO WHALE PROBLEM");
                                        }
                                    }
                                }
                                transaction!.IsIntegrated = true;
                            }
                            else
                            {
                                var transactionNotMapped = await _transactionNotMappedRepository.Add(new TransactionNotMapped
                                {
                                    Signature = transaction.Signature,
                                    WalletId = wallet?.ID,
                                    Link = "https://solscan.io/tx/" + transaction.Signature,
                                    Error = ETransactionType.INDEFINED.ToString(),
                                    StackTrace = null,
                                    DateTimeRunner = DateTime.Now
                                });
                                await this._transactionNotMappedRepository.DetachedItem(transactionNotMapped!);
                            }
                            transaction!.IsIntegrated = true;
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
                            transaction!.IsIntegrated = true;
                        }
                        this._transactionsRPCRecoveryRepository.Edit(transaction);
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
        private decimal? CalculatedTotalSol(string? tokenHash, decimal? amountSource, decimal? solPrice, decimal? tokenPrice, ETypeOperation? typeOperation)
        {
            switch (typeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SELL:
                    if (tokenHash == "So11111111111111111111111111111111111111112")
                        return Math.Abs(amountSource ?? 0);
                    else
                        return Math.Abs(amountSource / solPrice ?? 0);
                case ETypeOperation.SWAP:
                    return Math.Abs((amountSource * tokenPrice) / solPrice ?? 0);
                default:
                    break;
            }
            return null;
        }
        private decimal? CalculatedTotalUSD(string? tokenHash, decimal? amountSource, decimal? solPrice, decimal? tokenPrice, ETypeOperation? typeOperation)
        {
            switch (typeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SELL:
                    if (tokenHash != "So11111111111111111111111111111111111111112")
                        return Math.Abs(amountSource ?? 0);
                    else
                        return Math.Abs(amountSource * solPrice ?? 0);
                case ETypeOperation.SWAP:
                    return Math.Abs(amountSource * tokenPrice ?? 0);
                default:
                    break;
            }
            return null;
        }

        private async Task LoadClassWallets()
        {
            var classWallets = await _classWalletRepository.GetAll();
            foreach (var classWallet in classWallets)
                this.ClassWallets.Add(classWallet);
        }
        private async Task LoadWallets()
        {
            var wallets = await _walletRepository.GetAll();
            foreach (var wallet in wallets)
                this.Wallets.Add(wallet);
        }
    }
}
