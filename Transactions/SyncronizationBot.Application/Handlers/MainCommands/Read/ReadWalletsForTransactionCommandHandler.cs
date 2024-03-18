using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using System.Collections.Concurrent;

namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadWalletsForTransactionCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsForTransactionCommand, ReadWalletsForTransactionCommandResponse>
    {
        
        

        public ReadWalletsForTransactionCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            
        }
        public async Task<ReadWalletsForTransactionCommandResponse> Handle(ReadWalletsForTransactionCommand request, CancellationToken cancellationToken)
        {
            var walletsTracked = await GetWallets(x => x.IsActive == true && x.IsLoadBalance == true, x => x.UnixTimeSeconds!);
            if (walletsTracked?.Count() > 0) 
            {
                var finalTicks = base.GetFinalTicks();
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 30
                };
                await Parallel.ForEachAsync(walletsTracked, options, async (walletTracked, cancellationToken) =>
                {
                    var initialTicks = base.GetInitialTicks(walletTracked?.UnixTimeSeconds);
                    var response = await _mediator.Send(new RecoveryTransactionsCommand
                    {
                        WalletId = walletTracked?.ID,
                        WalletHash = walletTracked?.Hash,
                        InitialTicks = initialTicks,
                        FinalTicks = finalTicks
                    });
                });
                await this._walletRepository.SaveChangesASync();
                var response = await _mediator.Send(new RecoverySaveTransactionsCommand{ });
            }
            return new ReadWalletsForTransactionCommandResponse {  };
        }
        
    }
}