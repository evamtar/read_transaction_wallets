using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.Birdeye;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Application.Response.SolanaFM;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.CrossCutting.Birdeye.WalletPortifolio.Request;
using SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Request;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Domain.Service.CrossCutting.Birdeye;
using SyncronizationBot.Domain.Service.CrossCutting.Solanafm;
using System.Diagnostics;


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
            var wallet = await GetWallet(x => x.IsLoadBalance == false && x.IsActive == true);
            var hasNext = wallet != null;
            while (hasNext)
            {
                var finalTicks = GetInitialTicks(GetFinalTicks());
                var taskSFM = _mediator.Send(new RecoverySaveBalanceSFMCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                var taskByrdeye = _mediator.Send(new RecoverySaveBalanceBirdeyeCommand { WalletId = wallet?.ID, WalletHash = wallet?.Hash });
                await Task.WhenAll(taskSFM, taskByrdeye);
                wallet!.DateLoadBalance = taskByrdeye.Result.DateLoadBalance ?? taskSFM.Result.DateLoadBalance;
                wallet!.OldTransactionStared = wallet!.DateLoadBalance;
                wallet!.IsLoadBalance = true;
                await UpdateUnixTimeSeconds(finalTicks, wallet);
                wallet = await GetWallet(x => x.IsLoadBalance == false && x.IsActive == true);
                hasNext = wallet != null;
            }
            return new ReadWalletsBalanceCommandResponse { };
        }

    }
}
