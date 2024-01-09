using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.CrossCutting.AccountInfo.Request;
using ReadTransactionsWallets.Domain.Service.CrossCutting;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class ReadWalletsBalanceCommandHandler : IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IAccountInfoService _accountInfoService;
        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IAccountInfoService accountInfoService) 
        {
            this._mediator = mediator;
            this._accountInfoService = accountInfoService;
        }
        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var response = await this._accountInfoService.ExecuteRecoveryAccountInfoAsync(new AccountInfoRequest { WalletHash = "GhuBeitd7eh8KwCurXy1tFCRxGphpVxa8X4rUX8dQxHc" });
            return null!;
        }
    }
}
