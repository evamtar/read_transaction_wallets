﻿using MediatR;
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
using System.Xml;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class RecoverySaveTransactionsCommandHandler : IRequestHandler<RecoverySaveTransactionsCommand, RecoverySaveTransactionsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionsService _transactionsService;
        private readonly ITransfersService _transfersService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IOptions<MappedTokensConfig> _mappedTokensConfig;
        public RecoverySaveTransactionsCommandHandler(IMediator mediator,
                                                      ITokenRepository tokenRepository,
                                                      ITransactionsService transactionsService,
                                                      ITransfersService transfersService,
                                                      ITransactionsRepository transactionsRepository,
                                                      IOptions<MappedTokensConfig> mappedTokensConfig)
        {
            this._mediator = mediator;
            this._transactionsService = transactionsService;
            this._transfersService = transfersService;
            this._transactionsRepository = transactionsRepository;
            this._mappedTokensConfig = mappedTokensConfig;
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
                                var transferManager = await TransferManagerHelper.GetTransferManager(transactionDetails.Result.Data);
                                var transferAccount = TransferManagerHelper.GetTransferAccount(request.WalletHash, transactionDetails.Result.Data[0].Source, transferManager);
                                var transferInfo = TransferManagerHelper.GetTransferInfo(transferAccount, this._mappedTokensConfig.Value);
                                if (transferInfo.TransactionType == ETransactionType.INDEFINED)
                                { }
                                var tokenSended = (RecoverySaveTokenCommandResponse?)null;
                                var tokenReceived = (RecoverySaveTokenCommandResponse?)null;
                                try
                                {
                                    if (transferInfo?.TokenSended != null)
                                        tokenSended = await this._mediator.Send(new RecoverySaveTokenCommand{ TokenHash = transferInfo?.TokenSended?.Token });
                                    if (transferInfo?.TokenReceived != null)
                                        tokenReceived = await this._mediator.Send(new RecoverySaveTokenCommand{ TokenHash = transferInfo.TokenReceived?.Token });
                                    var transactionDB = await this._transactionsRepository.Add(new Transactions
                                    {
                                        Signature = transaction.Signature,
                                        DateOfTransaction = transaction.DateOfTransaction,
                                        AmountValueSource = (transferInfo?.TokenSended?.Amount ?? 0) / tokenSended?.Divisor ?? 1,
                                        AmountValueDestination = (transferInfo?.TokenReceived?.Amount ?? 0) / tokenReceived?.Divisor ?? 1,
                                        IdTokenSource = tokenSended?.TokenId,
                                        IdTokenDestination = tokenReceived?.TokenId,
                                        IdWallet = request.WalletId,
                                        TypeOperation = ((ETypeOperation)(int)(transferInfo?.TransactionType ?? ETransactionType.INDEFINED)),
                                        JsonResponse = null
                                    });
                                    await SendAlertTransacionForTelegram(request, transaction.Signature, transactionDB, transferInfo, tokenSended, tokenReceived);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message + " Signature: " + transaction.Signature);
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

        private async Task SendAlertTransacionForTelegram(RecoverySaveTransactionsCommand request, string? signature, Transactions? transaction, TransferInfo? transferInfo, RecoverySaveTokenCommandResponse? tokenSended, RecoverySaveTokenCommandResponse? tokenReceived)
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
                        var anotherTransactions = await this._transactionsRepository.FindFirstOrDefault(x => x.ID != transaction!.ID && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        if (anotherTransactions == null)
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.REBUY_MESSAGE,
                                                                                   new object[] {
                                                                                       signature ?? string.Empty,
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenReceived?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenHash ?? string.Empty,
                                                                                       tokenReceived?.MintAuthority ?? "NO",
                                                                                       tokenReceived?.FreezeAuthority ?? "NO",
                                                                                       (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                                                                                       CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                   })
                            });
                        }
                        else
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.BUY_MESSAGE,
                                                                                   new object[]
                                                                                   {
                                                                                       signature ?? string.Empty,
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenReceived?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenHash ?? string.Empty,
                                                                                       tokenReceived?.MintAuthority ?? "NO",
                                                                                       tokenReceived?.FreezeAuthority ?? "NO",
                                                                                       (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                                                                                       CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                   })
                            });

                        }
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.SELL_MESSAGE,
                                                                                new object[]
                                                                                {
                                                                                    signature ?? string.Empty,
                                                                                    request.WalletHash ?? string.Empty,
                                                                                    ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                    tokenSended?.TokenAlias ?? string.Empty,
                                                                                    CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                    CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                    transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                })
                        });
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.SWAP_MESSAGE,
                                                                                  new object[]
                                                                                  {
                                                                                       signature ?? string.Empty,
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenHash ?? string.Empty,
                                                                                       transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                  })
                        });
                    }
                    break;
                case EClassWalletAlert.MM:
                    if (transferInfo?.TransactionType == ETransactionType.BUY)
                    {
                        var anotherTransactions = await this._transactionsRepository.FindFirstOrDefault(x => x.ID != transaction!.ID && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        if (anotherTransactions == null)
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_NEW_BUY_MESSAGE,
                                                                                    new object[]
                                                                                    {
                                                                                        signature ?? string.Empty,
                                                                                        request.WalletHash ?? string.Empty,
                                                                                        ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                        tokenReceived?.TokenAlias ?? string.Empty,
                                                                                        tokenReceived?.TokenHash ?? string.Empty,
                                                                                        tokenReceived?.MintAuthority ?? "NO",
                                                                                        tokenReceived?.FreezeAuthority ?? "NO",
                                                                                        (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                                                                                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                        transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                    })
                            });
                        }
                        else
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_REBUY_MESSAGE,
                                                                                    new object[] {
                                                                                        signature ?? string.Empty,
                                                                                        request.WalletHash ?? string.Empty,
                                                                                        ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                        tokenReceived?.TokenAlias ?? string.Empty,
                                                                                        tokenReceived?.TokenHash ?? string.Empty,
                                                                                        tokenReceived?.MintAuthority ?? "NO",
                                                                                        tokenReceived?.FreezeAuthority ?? "NO",
                                                                                        (tokenReceived?.IsMutable ?? false) ? "YES" : "NO",
                                                                                        CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                        CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                        transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                    })
                            });
                        }
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SELL)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_SELL_MESSAGE,
                                                                                new object[]
                                                                                {
                                                                                    signature ?? string.Empty,
                                                                                    request.WalletHash ?? string.Empty,
                                                                                    ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                    tokenSended?.TokenAlias ?? string.Empty,
                                                                                    CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                    CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                    transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                })
                        });
                    }
                    else if (transferInfo?.TransactionType == ETransactionType.SWAP)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_SWAP_MESSAGE,
                                                                                  new object[]
                                                                                  {
                                                                                       signature ?? string.Empty,
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       CalculatedAmoutValue(transferInfo?.TokenSended?.Amount, tokenSended?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       CalculatedAmoutValue(transferInfo?.TokenReceived?.Amount, tokenReceived?.Divisor).ToString() + " " + tokenSended?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenAlias ?? string.Empty,
                                                                                       tokenReceived?.TokenHash ?? string.Empty,
                                                                                       transferInfo?.DataOfTransfer ?? DateTime.MinValue
                                                                                  })
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private decimal CalculatedAmoutValue(decimal? value, int? divisor) 
        {
            return (value / (divisor ?? 1)) ?? 0;
        }

        private async Task<Token> GetToken(string? tokenHash)
        {
            var token = await this._mediator.Send(new RecoverySaveTokenCommand { TokenHash = tokenHash });
            return new Token 
            { 
                ID = token.TokenId,
                Hash = tokenHash,
                Decimals = token.Decimals,
                TokenAlias = token.TokenAlias,
                FreezeAuthority = token.FreezeAuthority,
                MintAuthority = token.MintAuthority,
                IsMutable = token.IsMutable,
            };
        }
    }
}
