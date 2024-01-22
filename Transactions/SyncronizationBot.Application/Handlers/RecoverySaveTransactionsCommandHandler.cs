using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
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


namespace SyncronizationBot.Application.Handlers
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionNotMappedRepository _transactionNotMappedRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        private readonly IOptions<SyncronizationBotConfig> _readTransactionWalletsConfig;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITokenRepository tokenRepository,
                                                      ITransactionsService transactionsService,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository,
                                                      ITransactionNotMappedRepository transactionNotMappedRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig,
                                                      IOptions<SyncronizationBotConfig> readTransactionWalletsConfig)
        {
            this._mediator = mediator;
            this._transactionsService = transactionsService;
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._transactionNotMappedRepository = transactionNotMappedRepository;
            this._mappedTokensConfig = mappedTokensConfig;
            this._readTransactionWalletsConfig = readTransactionWalletsConfig;
        }

        public async Task<RecoverySaveTransactionsCommandResponse> Handle(RecoverySaveTransactionsCommand request, CancellationToken cancellationToken)
        {
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
                        foreach (var transaction in transactionResponse.Result!.Data)
                        {
                            var transactionDetails = await this._transfersService.ExecuteRecoveryTransfersAsync(new TransfersRequest { Signature = transaction.Signature });
                            if (transactionDetails.Result != null && transactionDetails.Result.Data?.Count > 0)
                            {
                                try
                                {
                                    var exists = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature == transaction.Signature);
                                    if (exists == null && request.DateLoadBalance < transactionDetails?.Result?.Data[0].DateOfTransfer)
                                    {
                                        var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails.Result.Data);
                                        var transferAccount = TransferManagerHelper.GetTransferAccount(request.WalletHash, transactionDetails.Result.Data[0].Source, transferManager);
                                        var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, this._mappedTokensConfig.Value);
                                        if (transferInfo.TransactionType != ETransactionType.INDEFINED)
                                        {
                                            var tokenSended = (RecoverySaveTokenCommandResponse?)null;
                                            var tokenSendedPool = (RecoverySaveTokenCommandResponse?)null;
                                            var tokenReceived = (RecoverySaveTokenCommandResponse?)null;
                                            var tokenReceivedPool = (RecoverySaveTokenCommandResponse?)null;

                                            if (transferInfo?.TokenSended != null)
                                                tokenSended = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSended?.Token });
                                            if (transferInfo?.TokenSendedPool != null)
                                                tokenSendedPool = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo?.TokenSendedPool?.Token });
                                            if (transferInfo?.TokenReceived != null)
                                                tokenReceived = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceived?.Token });
                                            if (transferInfo?.TokenReceivedPool != null)
                                                tokenReceivedPool = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = transferInfo.TokenReceivedPool?.Token });
                                            var transactionDB = await this._transactionsRepository.Add(new Transactions
                                            {
                                                Signature = transaction.Signature,
                                                DateOfTransaction = transaction.DateOfTransaction,
                                                AmountValueSource = CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor),
                                                AmountValueSourcePool = CalculatedAmoutValue(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Divisor),
                                                AmountValueDestination = (transferInfo?.TokenReceived?.Amount ?? 0) / tokenReceived?.Divisor ?? 1,
                                                AmountValueDestinationPool = CalculatedAmoutValue(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Divisor),
                                                IdTokenSource = tokenSended?.TokenId,
                                                IdTokenSourcePool = tokenSendedPool?.TokenId,
                                                IdTokenDestination = tokenReceived?.TokenId,
                                                IdTokenDestinationPool = tokenReceivedPool?.TokenId,
                                                IdWallet = request.WalletId,
                                                TypeOperation = ((ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED))
                                            });
                                            var balancePosition = await this.UpdateBalance(transactionDB, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool);
                                            await SendAlertTransacionForTelegram(request, transaction.Signature, transactionDB, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool);
                                        }
                                        else
                                        {
                                            await this._transactionNotMappedRepository.Add(new TransactionNotMapped
                                            {
                                                Signature = transaction.Signature,
                                                Link = "https://solscan.io/tx/" + transaction.Signature,
                                                Error = ETransactionType.INDEFINED.ToString(),
                                                StackTrace = null,
                                                DateTimeRunner = DateTime.Now
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await this._transactionNotMappedRepository.Add(new TransactionNotMapped
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
                }
                page++;
                hasNextPage = transactionResponse.Result?.Pagination?.TotalPages > page;
            }
            return new RecoverySaveTransactionsCommandResponse { };
        }

        private async Task<RecoveryAddUpdateBalanceItemCommandResponse> UpdateBalance(Transactions? transactions, TransferInfo? transferInfo, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool) 
        {
            switch (transactions?.TypeOperation)
            {
                case ETypeOperation.BUY:
                case ETypeOperation.SELL:
                case ETypeOperation.SWAP:
                    if (transferInfo?.PaymentFee.HasValue ?? false) 
                    {
                        var solTokenForFee = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" }); 
                        await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                        {
                            WalleId = transactions?.IdWallet,
                            TokenId = solTokenForFee?.TokenId,
                            Quantity = transferInfo?.PaymentFee / ((decimal?)solTokenForFee?.Divisor) ?? 1,
                            TokenHash = tokenSended?.Hash,
                        });
                    }
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    return await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                case ETypeOperation.SEND:
                    if (transferInfo?.PaymentFee > 0)
                    {
                        var solTokenForFee = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                        await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                        {
                            WalleId = transactions?.IdWallet,
                            TokenId = solTokenForFee?.TokenId,
                            Quantity = transferInfo?.PaymentFee,
                            TokenHash = tokenSended?.Hash,
                        });
                    }
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    break;
                case ETypeOperation.RECEIVED:
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                    break;
                case ETypeOperation.POOLCREATE:
                    if (transferInfo?.PaymentFee.HasValue ?? false)
                    {
                        var solTokenForFee = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                        await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                        {
                            WalleId = transactions?.IdWallet,
                            TokenId = solTokenForFee?.TokenId,
                            Quantity = transferInfo?.PaymentFee / ((decimal?)solTokenForFee?.Divisor) ?? 1,
                            TokenHash = tokenSended?.Hash,
                        });
                    }
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSource,
                        Quantity = transactions?.AmountValueSource,
                        TokenHash = tokenSended?.Hash,
                    });
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenSourcePool,
                        Quantity = transactions?.AmountValueSourcePool,
                        TokenHash = tokenSendedPool?.Hash,
                    });
                    break;
                case ETypeOperation.POOLFINALIZED:
                    if (transferInfo?.PaymentFee.HasValue ?? false)
                    {
                        var solTokenForFee = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = "So11111111111111111111111111111111111111112" });
                        await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                        {
                            WalleId = transactions?.IdWallet,
                            TokenId = solTokenForFee?.TokenId,
                            Quantity = transferInfo?.PaymentFee / ((decimal?)solTokenForFee?.Divisor) ?? 1,
                            TokenHash = tokenSended?.Hash,
                        });
                    }
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestination,
                        Quantity = transactions?.AmountValueDestination,
                        TokenHash = tokenReceived?.Hash,
                    });
                    await this._mediator.Send(new RecoveryAddUpdateBalanceItemCommand
                    {
                        WalleId = transactions?.IdWallet,
                        TokenId = transactions?.IdTokenDestinationPool,
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

        private async Task SendAlertTransacionForTelegram(RecoverySaveTransactionsCommand request, string? signature, Transactions? transaction, TransferInfo? transferInfo, RecoveryAddUpdateBalanceItemCommandResponse balancePosition, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool)
        {
            switch ((EClassWalletAlert)request.IdClassification!)
            {
                case EClassWalletAlert.None:
                    break;
                case EClassWalletAlert.Whale:
                case EClassWalletAlert.Asians:
                case EClassWalletAlert.Arbitrator:
                    if (transferInfo?.TransactionType == ETransactionType.BUY)
                    {
                        var anotherTransactions = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature != transaction!.Signature && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        await this.SendMessage(anotherTransactions == null ? ETypeMessage.BUY_MESSAGE : ETypeMessage.REBUY_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this.SendMessage(ETypeMessage.SELL_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this.SendMessage(ETypeMessage.SWAP_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLCREATE)
                    {
                        await this.SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLFINALIZED)
                    {
                        await this.SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    break;
                case EClassWalletAlert.MM:
                case EClassWalletAlert.BIG_BIG_WHALE:
                    if (transferInfo?.TransactionType == ETransactionType.BUY)
                    {
                        var anotherTransactions = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature != transaction!.Signature && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        await this.SendMessage(anotherTransactions == null ? ETypeMessage.MM_NEW_BUY_MESSAGE : ETypeMessage.MM_REBUY_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this.SendMessage(ETypeMessage.MM_SELL_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this.SendMessage(ETypeMessage.MM_SWAP_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLCREATE)
                    {
                        await this.SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLFINALIZED)
                    {
                        await this.SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, this.GetParametersArgsMessage(request, signature, transferInfo, balancePosition, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    break;
                default:
                    break;
            }
        }

        private object[] GetParametersArgsMessage(RecoverySaveTransactionsCommand request, string? signature, TransferInfo? transferInfo, RecoveryAddUpdateBalanceItemCommandResponse balancePosition, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool, ETransactionType transactionType)
        {
            switch (transactionType)
            {
                case ETransactionType.BUY:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        tokenReceived?.Name ?? string.Empty,
                        tokenReceived?.Hash ?? string.Empty,
                        tokenReceived?.MintAuthority ?? "NO",
                        tokenReceived?.FreezeAuthority ?? "NO",
                        (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.Symbol ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.Symbol ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                        (balancePosition.PercentModify ?? 0).ToString() + "%",
                        tokenReceived?.Hash ?? string.Empty
                };
                case ETransactionType.SELL:
                    return new object[] 
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        tokenSended?.Hash ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.Symbol ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.Symbol ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                        (balancePosition.PercentModify ?? 0).ToString() + "%",
                        tokenSended?.Hash ?? string.Empty
                    };
                case ETransactionType.SWAP:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.Symbol ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.Symbol ?? string.Empty,
                        tokenReceived?.Name ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                        (balancePosition.PercentModify ?? 0).ToString() + "%",
                        tokenReceived?.Hash ?? string.Empty,
                        tokenSended?.Hash ?? string.Empty
                    };
                case ETransactionType.POOLCREATE:
                    return new object[] 
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.Symbol ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Divisor).ToString() + " " + tokenSendedPool?.Symbol ?? string.Empty,
                        tokenSended?.Hash ?? string.Empty,
                        tokenSendedPool?.Hash ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                        tokenSended?.Hash ?? string.Empty,
                        tokenSendedPool?.Hash ?? string.Empty
                     };
                case ETransactionType.POOLFINALIZED:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.Symbol ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Divisor).ToString() + " " + tokenReceivedPool?.Symbol ?? string.Empty,
                        tokenReceived?.Hash ?? string.Empty,
                        tokenReceivedPool?.Hash?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer),
                        tokenReceived?.Hash ?? string.Empty,
                        tokenReceivedPool?.Hash ?? string.Empty
                    };
                default:
                    return null!;
            }
        }

        private async Task SendMessage(ETypeMessage typeMessage, object[] args) 
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolana,
                Message = TelegramMessageHelper.GetFormatedMessage(typeMessage, args)
            });
        }

        private decimal? CalculatedAmoutValue(decimal? value, int? divisor) 
        {
            if (value == null || divisor == null) return null;
            return (value / (divisor ?? 1)) ?? 0;
        }

        private DateTime AdjustDateTimeToPtBR(DateTime? dateTime) 
        {
            return dateTime?.AddHours(this._readTransactionWalletsConfig.Value.GTMHoursAdjust?? 0) ?? DateTime.MinValue; 
        }
        
    }
}
