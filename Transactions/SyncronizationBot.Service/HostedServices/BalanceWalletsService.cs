using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Service.InternalService.Wallet;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class BalanceWalletsService : BaseHostedService
    {
        protected override IOptions<SyncronizationBotConfig>? Options => throw new NotImplementedException();
        protected override ETypeService? TypeService => ETypeService.Balance;

        private readonly IWalletService _walletService;
        public BalanceWalletsService(IMediator mediator,
                                     IWalletService walletService) : base(mediator)
        {
            this._walletService = walletService;
        }


        protected override async Task DoExecute(CancellationToken cancellationToken)
        {
            using (var service = this._walletService) 
            {
                var wallets = await service.Get(x => x.IsLoadBalance == false && x.IsActive == true);
                if (wallets?.Count() > 0)
                {
                    foreach (var wallet in wallets)
                    {
                        //var balanceSFM = await _mediator.Send(new RecoverySaveBalanceSFMCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                        //var balanceByrdeye = await _mediator.Send(new RecoverySaveBalanceBirdeyeCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                        //wallet!.DateLoadBalance = balanceByrdeye.DateLoadBalance ?? balanceSFM.DateLoadBalance ?? DateTime.Now;
                        //wallet!.IsLoadBalance = true;
                    }
                }
            }
        }
    }
}
