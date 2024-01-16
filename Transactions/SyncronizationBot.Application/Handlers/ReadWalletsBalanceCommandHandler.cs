using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Service.CrossCutting;

namespace SyncronizationBot.Application.Handlers
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
