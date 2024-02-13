using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadWalletsCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IClassWalletRepository _classWalletRepository;

        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository,
                                         IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            _classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandResponse> Handle(ReadWalletsCommand request, CancellationToken cancellationToken)
        {
            var hasWalletsWithBalanceLoad = false;
            var walletsTracked = await GetWallets(x => x.IsActive == true && x.IsLoadBalance == true, x => x.UnixTimeSeconds!);
            if(walletsTracked?.Count() > 0) 
            {
                foreach (var walletTracked in walletsTracked)
                {
                    hasWalletsWithBalanceLoad = true;
                    var initialTicks = GetInitialTicks(walletTracked?.UnixTimeSeconds);
                    var finalTicks = GetFinalTicks();
                    if (initialTicks > finalTicks)
                        initialTicks -= (initialTicks - finalTicks) * 2;
                    var classWallet = await _classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked!.ClassWalletId);
                    var response = await _mediator.Send(new RecoverySaveTransactionsCommand
                    {
                        WalletId = walletTracked?.ID,
                        WalletHash = walletTracked?.Hash,
                        ClassWallet = classWallet,
                        DateLoadBalance = walletTracked?.DateLoadBalance,
                        IsContingecyTransactions = request?.IsContingecyTransactions,
                        InitialTicks = initialTicks,
                        FinalTicks = finalTicks
                    });
                    this.TotalValidTransactions += response.TotalValidTransactions ?? 0;
                    walletTracked!.LastUpdate = DateTime.Now;
                    await _classWalletRepository.DetachedItem(classWallet!);
                    await UpdateUnixTimeSeconds(finalTicks, walletTracked);
                    //Por causa da caceta do rate limit da solana FM
                    await Task.Delay(new Random().Next(20, 80));
                }
            }
            return new ReadWalletsCommandResponse { TotalValidTransactions = this.TotalValidTransactions, HasWalletsWithBalanceLoad = hasWalletsWithBalanceLoad };
        }
    }
}