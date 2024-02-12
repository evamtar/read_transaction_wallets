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
                int totalWalletPerRange = 30;
                var rest = wallets.Count % totalWalletPerRange;
                var total = ((int)wallets.Count / totalWalletPerRange) + (rest > 0 ? 1 : 0);
                for (int i = 0; i < total; i++)
                {
                    int initRange = (totalWalletPerRange * i);
                    var walletsToTask = wallets.GetRange(initRange, totalWalletPerRange);
                    await Parallel.ForEachAsync(walletsToTask, async (wallet, cancellationToken) =>
                    {
                        var finalTicks = GetInitialTicks(GetFinalTicks());
                        var taskSFM = _mediator.Send(new RecoverySaveBalanceSFMCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                        var taskByrdeye = _mediator.Send(new RecoverySaveBalanceBirdeyeCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                        await Task.WhenAll(taskSFM, taskByrdeye);
                        wallet!.DateLoadBalance = taskByrdeye.Result.DateLoadBalance ?? taskSFM.Result.DateLoadBalance ?? DateTime.Now;
                        wallet!.OldTransactionStared = wallet!.DateLoadBalance;
                        wallet!.IsLoadBalance = true;
                        await UpdateUnixTimeSeconds(finalTicks, wallet);
                    });
                }
            }
            return new ReadWalletsBalanceCommandResponse { };
        }
    }
}
