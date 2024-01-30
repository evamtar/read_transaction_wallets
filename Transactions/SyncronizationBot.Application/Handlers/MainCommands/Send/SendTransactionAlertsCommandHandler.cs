using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendTransactionAlertsCommandHandler : IRequestHandler<SendTransactionAlertsCommand, SendTransactionAlertsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private readonly IClassWalletRepository _classWalletRepository;
        public SendTransactionAlertsCommandHandler(IMediator mediator,
                                                   IWalletBalanceHistoryRepository walletBalanceHistoryRepository,
                                                   IClassWalletRepository classWalletRepository)
        {
            this._mediator = mediator;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
            this._classWalletRepository = classWalletRepository;
        }

        public async Task<SendTransactionAlertsCommandResponse> Handle(SendTransactionAlertsCommand request, CancellationToken cancellationToken)
        {
            /*
             * SUBSTITUIR
             */
            await this.SendAlertTransacionForTelegram(request);
            return new SendTransactionAlertsCommandResponse { };
        }

        private async Task SendAlertTransacionForTelegram(SendTransactionAlertsCommand request)
        {
            switch ((EClassWalletAlert)request.IdClassification!)
            {
                case EClassWalletAlert.None:
                    break;
                case EClassWalletAlert.Whale:
                case EClassWalletAlert.Asians:
                case EClassWalletAlert.Arbitrator:
                    if (request?.Transactions?.TypeOperation == ETypeOperation.BUY)
                        await SendAlertBuyOrRebuy(request, ETypeMessage.BUY_MESSAGE, ETypeMessage.REBUY_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.SELL)
                        await SendAlertAnotherTransaction(request, ETypeMessage.SELL_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.SWAP)
                        await SendAlertAnotherTransaction(request, ETypeMessage.SWAP_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.POOLCREATE)
                        await SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, GetParametersArgsMessage(request));
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.POOLFINALIZED)
                        await SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, GetParametersArgsMessage(request));
                    break;
                case EClassWalletAlert.MM:
                case EClassWalletAlert.BIG_BIG_WHALE:
                    if (request?.Transactions?.TypeOperation == ETypeOperation.BUY)
                        await SendAlertBuyOrRebuy(request, ETypeMessage.MM_NEW_BUY_MESSAGE, ETypeMessage.MM_REBUY_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.SELL)
                        await SendAlertAnotherTransaction(request, ETypeMessage.MM_SELL_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.SWAP)
                        await SendAlertAnotherTransaction(request, ETypeMessage.MM_SWAP_MESSAGE);
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.POOLCREATE)
                        await SendMessage(ETypeMessage.POOL_CREATED_MESSAGE, GetParametersArgsMessage(request));
                    else if (request?.Transactions?.TypeOperation == ETypeOperation.POOLFINALIZED)
                        await SendMessage(ETypeMessage.POOL_FINALIZED_MESSAGE, GetParametersArgsMessage(request));
                    break;
                default:
                    break;
            }
        }

        private async Task SendAlertAnotherTransaction(SendTransactionAlertsCommand? request,
                                         ETypeMessage typeMessage)
        {
            if ((request?.TokensMapped?.Contains(request?.TokenSendedHash!) ?? false) && (request?.TokensMapped?.Contains(request?.TokenReceivedHash!) ?? false))
                return;
            await SendMessage(typeMessage, GetParametersArgsMessage(request));
        }

        private async Task SendAlertBuyOrRebuy(SendTransactionAlertsCommand? request,
                                               ETypeMessage buyMessage,
                                               ETypeMessage rebuyMessage)
        {
            if ((request?.TokensMapped?.Contains(request?.TokenSendedHash!) ?? false) && (request?.TokensMapped?.Contains(request?.TokenReceivedHash!) ?? false))
                return;
            var existsTokenWallet = await _walletBalanceHistoryRepository.FindFirstOrDefault(x => x.TokenHash != request!.TokenReceivedHash && x.IdWallet == request!.WalletId && x.Signature != request!.Transactions!.Signature);
            await SendMessage(existsTokenWallet == null ? buyMessage : rebuyMessage, GetParametersArgsMessage(request));
        }

        private object[] GetParametersArgsMessage(SendTransactionAlertsCommand? request)
        {
            switch (request?.Transactions?.TypeOperation)
            {
                case ETypeOperation.BUY:
                    return new object[]
                    {
                        request?.Transactions.Signature ?? string.Empty,
                        request?.WalletHash ?? string.Empty,
                        this.GetClassificationDescription(request?.IdClassification),
                        request?.TokenReceivedName ?? string.Empty,
                        request?.TokenReceivedHash ?? string.Empty,
                        request?.TokenReceivedMintAuthority ?? "NO",
                        request?.TokenReceivedFreezeAuthority ?? "NO",
                        request?.TokenReceivedIsMutable ?? false ? "YES" : "NO",
                        request?.Transactions?.AmountValueDestination.ToString() + " " + request?.TokenReceivedSymbol ?? string.Empty,
                        request?.Transactions?.AmountValueSource.ToString() + " " + request?.TokenSendedSymbol ?? string.Empty,
                        request?.DateOfTransfer ?? DateTime.Now,
                        (request?.PercentModify ?? 0).ToString() + "%",
                        request?.TokenReceivedHash ?? string.Empty
                };
                case ETypeOperation.SELL:
                    return new object[]
                    {
                        request?.Transactions.Signature ?? string.Empty,
                        request?.WalletHash ?? string.Empty,
                        this.GetClassificationDescription(request?.IdClassification),
                        request?.TokenSendedHash ?? string.Empty,
                        request?.Transactions?.AmountValueSource.ToString() + " " + request?.TokenSendedSymbol ?? string.Empty,
                        request?.Transactions?.AmountValueDestination.ToString() + " " + request?.TokenReceivedSymbol ?? string.Empty,
                        request?.DateOfTransfer ?? DateTime.Now,
                        (request?.PercentModify ?? 0).ToString() + "%",
                        request?.TokenSendedHash ?? string.Empty
                    };
                case ETypeOperation.SWAP:
                    return new object[]
                    {
                        request?.Transactions.Signature ?? string.Empty,
                        request?.WalletHash ?? string.Empty,
                        this.GetClassificationDescription(request?.IdClassification),
                        request?.Transactions?.AmountValueSource.ToString() + " " + request?.TokenSendedSymbol ?? string.Empty,
                        request?.Transactions?.AmountValueDestination.ToString() + " " + request?.TokenReceivedSymbol ?? string.Empty,
                        request?.TokenReceivedName ?? string.Empty,
                        request?.DateOfTransfer ?? DateTime.Now,
                        (request?.PercentModify ?? 0).ToString() + "%",
                        request?.TokenReceivedHash ?? string.Empty,
                        request?.TokenSendedHash ?? string.Empty
                    };
                case ETypeOperation.POOLCREATE:
                    return new object[]
                    {
                        request?.Transactions.Signature ?? string.Empty,
                        request?.WalletHash ?? string.Empty,
                        this.GetClassificationDescription(request?.IdClassification),
                        request?.Transactions?.AmountValueSource.ToString() + " " + request?.TokenSendedSymbol ?? string.Empty,
                        request?.Transactions?.AmountValueSourcePool.ToString() + " " + request?.TokenSendedPoolSymbol ?? string.Empty,
                        request?.TokenSendedHash ?? string.Empty,
                        request?.TokenSendedPoolHash ?? string.Empty,
                        request?.DateOfTransfer ?? DateTime.Now,
                        request?.TokenSendedHash ?? string.Empty,
                        request?.TokenSendedPoolHash ?? string.Empty
                     };
                case ETypeOperation.POOLFINALIZED:
                    return new object[]
                    {
                        request?.Transactions.Signature ?? string.Empty,
                        request?.WalletHash ?? string.Empty,
                        this.GetClassificationDescription(request?.IdClassification),
                        request?.Transactions?.AmountValueDestination.ToString() + " " + request?.TokenReceivedSymbol ?? string.Empty,
                        request?.Transactions?.AmountValueDestinationPool.ToString() + " " + request?.TokenReceivedPoolSymbol ?? string.Empty,
                        request?.TokenReceivedHash ?? string.Empty,
                        request?.TokenReceivedPoolHash ?? string.Empty,
                        request?.DateOfTransfer ?? DateTime.Now,
                        request?.TokenReceivedHash ?? string.Empty,
                        request?.TokenReceivedPoolHash ?? string.Empty
                    };
                default:
                    return null!;
            }
        }

        private async Task SendMessage(ETypeMessage typeMessage, object[] args)
        {
            await _mediator.Send(new SendTelegramMessageCommand
            {
                Channel = ETelegramChannel.CallSolana,
                Message = TelegramMessageHelper.GetFormatedMessage(typeMessage, args)
            });
        }

        private async Task<string?> GetClassificationDescription(int? idClassification) 
        { 
            var classification = await this._classWalletRepository.FindFirstOrDefault(x => x.IdClassification == idClassification);
            if(classification != null) 
                return classification.Description ?? string.Empty;
            return string.Empty;
        }

    }
}
