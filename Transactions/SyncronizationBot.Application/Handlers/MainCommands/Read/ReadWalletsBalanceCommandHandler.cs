using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.Birdeye;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;



namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadWalletsBalanceCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsBalanceCommand, ReadWalletsBalanceCommandResponse>
    {

        public ReadWalletsBalanceCommandHandler(IMediator mediator,
                                                IWalletRepository walletRepository,
                                                IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {

        }

        public async Task<ReadWalletsBalanceCommandResponse> Handle(ReadWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var wallets = await GetWallets(x => x.IsLoadBalance == false && x.IsActive == true);
            if(wallets?.Count() > 0) 
            {
                foreach (var wallet in wallets)
                {
                    var finalTicks = GetInitialTicks(GetFinalTicks());
                    var balanceSFM = await _mediator.Send(new RecoverySaveBalanceSFMCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                    var balanceByrdeye = await _mediator.Send(new RecoverySaveBalanceBirdeyeCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                    wallet!.DateLoadBalance = balanceByrdeye.DateLoadBalance ?? balanceSFM.DateLoadBalance ?? DateTime.Now;
                    wallet!.OldTransactionStared = wallet!.DateLoadBalance;
                    wallet!.IsLoadBalance = true;
                    await UpdateUnixTimeSeconds(finalTicks, wallet);
                }                
                    
            }
            return new ReadWalletsBalanceCommandResponse { };
        }
    }
}
