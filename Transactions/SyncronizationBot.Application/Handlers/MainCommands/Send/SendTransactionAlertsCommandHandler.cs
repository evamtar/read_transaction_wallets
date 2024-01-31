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
                Parameters = request?.Parameters,
                TypeAlert = await this.TransalateTypeOperationInTypeAlert(request)
            });
            return new SendTransactionAlertsCommandResponse { };
        }

        private async Task<ETypeAlert> TransalateTypeOperationInTypeAlert(SendTransactionAlertsCommand? request)
        {
            switch (request!.Transactions!.TypeOperation)
            {
                case ETypeOperation.BUY:
                    if ((request?.TokensMapped?.Contains(request?.TokenSendedHash!) ?? false) && (request?.TokensMapped?.Contains(request?.TokenReceivedHash!) ?? false))
                        return ETypeAlert.NONE;
                    var existsTokenWallet = await _walletBalanceHistoryRepository.FindFirstOrDefault(x => x.TokenHash != request!.TokenReceivedHash && x.IdWallet == request!.WalletId && x.Signature != request!.Transactions!.Signature);
                    return existsTokenWallet == null ? ETypeAlert.BUY : ETypeAlert.REBUY;
                case ETypeOperation.SELL:
                case ETypeOperation.SWAP:
                case ETypeOperation.POOLCREATE:
                case ETypeOperation.POOLFINALIZED:
                    return ((ETypeAlert)((int)request!.Transactions!.TypeOperation) + 1);
                default:
                    return ETypeAlert.NONE;
            }
        }

    }
}
