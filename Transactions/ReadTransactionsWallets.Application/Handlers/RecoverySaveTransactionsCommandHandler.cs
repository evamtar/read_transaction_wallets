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
                            if (transactionDetails.Result != null)
                            {
                                if (transactionDetails.Result.Data?.Count > 0)
                                {
                                    var transfersCompose = TransferComposeHelper.GetTransfersCompose(request.WalletHash, TransferDocumentHelper.CreateTransferDocument(transactionDetails.Result.Data), this._mappedTokensConfig.Value);
                                    if (transfersCompose != null) 
                                    {
                                        foreach (var transferCompose in transfersCompose)
                                        {
                                            var tokenFrom = (RecoverySaveTokenCommandResponse?)null;
                                            if (transferCompose.TypeOperation != ETypeOperation.Send && transferCompose.TypeOperation != ETypeOperation.Received)
                                            {
                                                tokenFrom = await this._mediator.Send(new RecoverySaveTokenCommand
                                                {
                                                    TokenHash = string.IsNullOrEmpty(transferCompose?.TransactionSended?.Token) ? "So11111111111111111111111111111111111111112" : transferCompose?.TransactionSended?.Token
                                                });
                                            }
                                            var tokenTO = await this._mediator.Send(new RecoverySaveTokenCommand
                                            {
                                                TokenHash = string.IsNullOrEmpty(transferCompose?.TransactionReceived?.Token) ? "So11111111111111111111111111111111111111112" : transferCompose?.TransactionReceived?.Token
                                            });
                                            try
                                            {
                                                var transactionDB = await this._transactionsRepository.Add(new Transactions
                                                {
                                                    Signature = transaction.Signature,
                                                    DateOfTransaction = transaction.DateOfTransaction,
                                                    AmountValueSource = (transferCompose?.TransactionSended?.Amount ?? 0) / tokenFrom?.Divisor ?? 1,
                                                    AmountValueDestination = (transferCompose?.TransactionReceived?.Amount ?? 0) / tokenTO?.Divisor ?? 1,
                                                    IdTokenSource = tokenFrom?.TokenId,
                                                    IdTokenDestination = tokenTO?.TokenId,
                                                    IdWallet = request.WalletId,
                                                    TypeOperation = transferCompose?.TypeOperation ?? ETypeOperation.None,
                                                    JsonResponse = null
                                                });
                                                await SendAlertTransacionForTelegram(request, transactionDB, transferCompose, tokenFrom, tokenTO);
                                            }
                                            catch (Exception ex) 
                                            {
                                                throw new Exception(ex.Message + " Signature: " + transaction.Signature);
                                            }
                                        }
                                    }
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

        private async Task SendAlertTransacionForTelegram(RecoverySaveTransactionsCommand request, Transactions? transaction, TransferCompose? transferCompose, RecoverySaveTokenCommandResponse? tokenFrom, RecoverySaveTokenCommandResponse? tokenTO)
        {
            switch ((EClassWalletAlert)request.IdClassification!)
            {
                case EClassWalletAlert.None:
                    break;
                case EClassWalletAlert.Whale:
                case EClassWalletAlert.Asians:
                case EClassWalletAlert.Arbitrator:
                    if (transferCompose?.TypeOperation == ETypeOperation.Buy)
                    {
                        var anotherTransactions = this._transactionsRepository.FindFirstOrDefault(x => x.ID != transaction!.ID && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        if (anotherTransactions != null)
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.REBUY_MESSAGE,
                                                                                   new object[] {
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenTO?.TokenAlias ?? string.Empty,
                                                                                       tokenTO?.TokenHash ?? string.Empty,
                                                                                       tokenTO?.MintAuthority ?? "NO",
                                                                                       tokenTO?.FreezeAuthority ?? "NO",
                                                                                       (tokenTO?.IsMutable ?? false) ? "YES" : "NO",
                                                                                       (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0,
                                                                                       ((transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 1).ToString() + " " +tokenFrom?.TokenAlias ?? string.Empty,
                                                                                       transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
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
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenTO?.TokenAlias ?? string.Empty,
                                                                                       tokenTO?.TokenHash ?? string.Empty,
                                                                                       tokenTO?.MintAuthority ?? "NO",
                                                                                       tokenTO?.FreezeAuthority ?? "NO",
                                                                                       (tokenTO?.IsMutable ?? false) ? "YES" : "NO",
                                                                                       (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0,
                                                                                       ((transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 1).ToString() + " " + tokenFrom?.TokenAlias ?? string.Empty,
                                                                                       transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                   })
                            });

                        }
                    }
                    else if (transferCompose?.TypeOperation == ETypeOperation.Sell)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.SELL_MESSAGE,
                                                                                new object[]
                                                                                {
                                                                                    request.WalletHash ?? string.Empty,
                                                                                    ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                    tokenFrom?.TokenAlias ?? string.Empty,
                                                                                    (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0,
                                                                                    ((transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 1).ToString() + " " + tokenTO?.TokenAlias ?? string.Empty,
                                                                                    transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                })
                        });
                    }
                    else if (transferCompose?.TypeOperation == ETypeOperation.Swap) 
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.SWAP_MESSAGE,
                                                                                  new object[]
                                                                                  {
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenFrom?.TokenAlias ?? string.Empty,
                                                                                       (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0,
                                                                                       tokenTO?.TokenAlias ?? string.Empty,
                                                                                       tokenTO?.TokenHash ?? string.Empty,
                                                                                       transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                  })
                        });
                    }
                    break;
                case EClassWalletAlert.MM:
                    if (transferCompose?.TypeOperation == ETypeOperation.Buy)
                    {
                        var anotherTransactions = this._transactionsRepository.FindFirstOrDefault(x => x.ID != transaction!.ID && x.IdWallet == request.WalletId && x.IdTokenDestination == transaction!.IdTokenDestination);
                        if (anotherTransactions != null)
                        {
                            await this._mediator.Send(new SendTelegramMessageCommand
                            {
                                Channel = EChannel.CallSolana,
                                Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_NEW_BUY_MESSAGE,
                                                                                    new object[]
                                                                                    {
                                                                                        request.WalletHash ?? string.Empty,
                                                                                        ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                        tokenTO?.TokenAlias ?? string.Empty,
                                                                                        tokenTO?.TokenHash ?? string.Empty,
                                                                                        tokenTO?.MintAuthority ?? "NO",
                                                                                        tokenTO?.FreezeAuthority ?? "NO",
                                                                                        (tokenTO?.IsMutable ?? false) ? "YES" : "NO",
                                                                                        (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0,
                                                                                        ((transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 1).ToString() + " " + tokenFrom?.TokenAlias ?? string.Empty,
                                                                                        transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
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
                                                                                        request.WalletHash ?? string.Empty,
                                                                                        ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                        tokenTO?.TokenAlias ?? string.Empty,
                                                                                        tokenTO?.TokenHash ?? string.Empty,
                                                                                        tokenTO?.MintAuthority ?? "NO",
                                                                                        tokenTO?.FreezeAuthority ?? "NO",
                                                                                        (tokenTO?.IsMutable ?? false) ? "YES" : "NO",
                                                                                        (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0,
                                                                                        ((transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 1).ToString() + " " + tokenFrom?.TokenAlias ?? string.Empty,
                                                                                        transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                    })
                            });
                        }
                    }
                    else if (transferCompose?.TypeOperation == ETypeOperation.Sell)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_SELL_MESSAGE,
                                                                                new object[]
                                                                                {
                                                                                    request.WalletHash ?? string.Empty,
                                                                                    ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                    tokenFrom?.TokenAlias ?? string.Empty,
                                                                                    (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0,
                                                                                    ((transferCompose?.TransactionReceived?.Amount / (tokenTO?.Divisor ?? 1)) ?? 0 / (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 1).ToString() + " " + tokenTO?.TokenAlias ?? string.Empty,
                                                                                    transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                })
                        });
                    }
                    else if (transferCompose?.TypeOperation == ETypeOperation.Swap)
                    {
                        await this._mediator.Send(new SendTelegramMessageCommand
                        {
                            Channel = EChannel.CallSolana,
                            Message = TelegramMessageHelper.GetFormatedMessage(ETypeMessage.MM_SWAP_MESSAGE,
                                                                                  new object[]
                                                                                  {
                                                                                       request.WalletHash ?? string.Empty,
                                                                                       ((EClassWalletAlert)request.IdClassification).ToString(),
                                                                                       tokenFrom?.TokenAlias ?? string.Empty,
                                                                                       (transferCompose?.TransactionSended?.Amount / (tokenFrom?.Divisor ?? 1)) ?? 0,
                                                                                       tokenTO?.TokenAlias ?? string.Empty,
                                                                                       tokenTO?.TokenHash ?? string.Empty,
                                                                                       transferCompose?.TransactionReceived?.DateOfTransfer ?? DateTime.MinValue
                                                                                  })
                        });
                    }
                    break;
                default:
                    break;
            }
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
