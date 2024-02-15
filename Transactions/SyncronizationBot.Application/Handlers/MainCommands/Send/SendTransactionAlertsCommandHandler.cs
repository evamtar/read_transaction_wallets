using MediatR;
using SyncronizationBot.Application.Commands.MainCommands.Send;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Application.Response.MainCommands.Send;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Model.Utils.Transfer;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers.MainCommands.Send
{
    public class SendTransactionAlertsCommandHandler : IRequestHandler<SendTransactionAlertsCommand, SendTransactionAlertsCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        
        public SendTransactionAlertsCommandHandler(IMediator mediator,
                                                   IWalletBalanceHistoryRepository walletBalanceHistoryRepository)
        {
            this._mediator = mediator;
            this._walletBalanceHistoryRepository = walletBalanceHistoryRepository;
        }

        public async Task<SendTransactionAlertsCommandResponse> Handle(SendTransactionAlertsCommand request, CancellationToken cancellationToken)
        {
            await this._mediator.Send(new SendAlertMessageCommand
            {
                EntityId = request?.EntityId,
                Parameters = request?.Parameters,
                TypeAlert = await this.TransalateTypeOperationInTypeAlert(request)
            });
            return new SendTransactionAlertsCommandResponse { };
        }

        private async Task<ETypeAlert> TransalateTypeOperationInTypeAlert(SendTransactionAlertsCommand? request)
        {
            ///TODO:Evandro
            //switch (request!.Transactions!.TypeOperation)
            //{
            //    case ETypeOperation.BUY:
            //        if ((request?.TokensMapped?.Contains(request?.TokenSendedHash!) ?? false) && (request?.TokensMapped?.Contains(request?.TokenReceivedHash!) ?? false))
            //            return ETypeAlert.NONE;
            //        var existsTokenWallet = await _walletBalanceHistoryRepository.FindFirstOrDefault(x => x.TokenHash == request!.TokenReceivedHash && x.WalletId == request!.WalletId && x.Signature != request!.Transactions!.Signature);
            //        return existsTokenWallet == null ? ETypeAlert.BUY : ETypeAlert.REBUY;
            //    case ETypeOperation.SELL:
            //        return ETypeAlert.SELL;
            //    case ETypeOperation.SWAP:
            //        return ETypeAlert.SWAP;
            //    case ETypeOperation.POOLCREATE:
            //        return ETypeAlert.POOL_CREATE;
            //    case ETypeOperation.POOLFINALIZED:
            //        return ETypeAlert.POOL_FINISH;
            //    default:
            //        return ETypeAlert.NONE;
            //}
            return ETypeAlert.NONE;
        }

    }
}
