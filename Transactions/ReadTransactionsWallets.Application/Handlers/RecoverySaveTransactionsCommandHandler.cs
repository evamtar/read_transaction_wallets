using MediatR;
using Microsoft.Extensions.Options;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.Configs;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Request;
using ReadTransactionsWallets.Domain.Model.CrossCutting.Transfers.Response;
using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Model.Enum;
using ReadTransactionsWallets.Domain.Model.Utils.Helpers;
using ReadTransactionsWallets.Domain.Model.Utils.Transfer;
using ReadTransactionsWallets.Domain.Repository;
using ReadTransactionsWallets.Domain.Service.CrossCutting;
using ReadTransactionsWallets.Utils;
using System.Transactions;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ITransactionNotMappedRepository _transactionNotMappedRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        private readonly IOptions<ReadTransactionWalletsConfig> _readTransactionWalletsConfig;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITokenRepository tokenRepository,
                                                      ITransactionsService transactionsService,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository,
                                                      ITransactionNotMappedRepository transactionNotMappedRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig,
                                                      IOptions<ReadTransactionWalletsConfig> readTransactionWalletsConfig)
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
                                        await SendAlertTransacionForTelegram(request, transaction.Signature, transactionDB, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool);
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

        private async Task SendAlertTransacionForTelegram(RecoverySaveTransactionsCommand request, string? signature, Transactions? transaction, TransferInfo? transferInfo, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool)
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
                        await this.SendMessage(anotherTransactions == null ? ETypeMessage.BUY_MESSAGE : ETypeMessage.REBUY_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this.SendMessage(ETypeMessage.SELL_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this.SendMessage(ETypeMessage.SWAP_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLCREATE)
                    {
                        await this.SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLFINALIZED)
                    {
                        await this.SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    break;
                case EClassWalletAlert.MM:
                    if (transferInfo?.TransactionType == ETransactionType.BUY)
                    {
                        var anotherTransactions = await this._transactionsRepository.FindFirstOrDefault(x => x.Signature != transaction!.Signature && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        await this.SendMessage(anotherTransactions == null ? ETypeMessage.MM_NEW_BUY_MESSAGE : ETypeMessage.MM_REBUY_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this.SendMessage(ETypeMessage.MM_SELL_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this.SendMessage(ETypeMessage.MM_SWAP_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLCREATE)
                    {
                        await this.SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.POOLFINALIZED)
                    {
                        await this.SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, this.GetParametersArgsMessage(request, signature, transaction, transferInfo, tokenSended, tokenSendedPool, tokenReceived, tokenReceivedPool, transferInfo!.TransactionType));
                    }
                    break;
                default:
                    break;
            }
        }

        private object[] GetParametersArgsMessage(RecoverySaveTransactionsCommand request, string? signature, Transactions? transaction, TransferInfo? transferInfo, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenSendedPool, RecoverySaveTokenCommandResponse? tokenReceived, RecoverySaveTokenCommandResponse? tokenReceivedPool, ETransactionType transactionType)
        {
            switch (transactionType)
            {
                case ETransactionType.BUY:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        tokenReceived?.TokenAlias ?? string.Empty,
                        tokenReceived?.TokenHash ?? string.Empty,
                        tokenReceived?.MintAuthority ?? "NO",
                        tokenReceived?.FreezeAuthority ?? "NO",
                        (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer)
                };
                case ETransactionType.SELL:
                    return new object[] 
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        tokenSended?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.TokenAlias ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer)
                    };
                case ETransactionType.SWAP:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.TokenAlias ?? string.Empty,
                        tokenReceived?.TokenHash ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer)
                    };
                case ETransactionType.POOLCREATE:
                    return new object[] 
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenSendedPool?.Amount, tokenSendedPool?.Divisor).ToString() + " " + tokenSendedPool?.TokenAlias ?? string.Empty,
                        tokenSended?.TokenHash ?? string.Empty,
                        tokenSendedPool?.TokenHash ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer)
                     };
                case ETransactionType.POOLFINALIZED:
                    return new object[]
                    {
                        signature ?? string.Empty,
                        request.WalletHash ?? string.Empty,
                        ((EClassWalletAlert)request.IdClassification!).ToString(),
                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenReceived?.TokenAlias ?? string.Empty,
                        CalculatedAmoutValue(transferInfo?.TokenReceivedPool?.Amount, tokenReceivedPool?.Divisor).ToString() + " " + tokenReceivedPool?.TokenAlias ?? string.Empty,
                        tokenReceived?.TokenHash ?? string.Empty,
                        tokenReceivedPool?.TokenHash ?? string.Empty,
                        AdjustDateTimeToPtBR(transferInfo?.DataOfTransfer)
                    };
                default:
                    return null;
            }
        }

        private async Task SendMessage(ETypeMessage typeMessage, object[] args) 
        {
            await this._mediator.Send(new SendTelegramMessageCommand
            {
                Channel = EChannel.CallSolana,
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
