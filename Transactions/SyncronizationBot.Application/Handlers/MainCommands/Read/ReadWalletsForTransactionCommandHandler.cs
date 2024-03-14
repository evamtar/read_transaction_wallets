using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
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
        private ConcurrentBag<ClassWallet> ClassWallets = new ConcurrentBag<ClassWallet>();
        private readonly IClassWalletRepository _classWalletRepository;

        public ReadWalletsForTransactionCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository,
                                         IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsForTransactionCommandResponse> Handle(ReadWalletsForTransactionCommand request, CancellationToken cancellationToken)
        {
            var hasWalletsWithBalanceLoad = false;
            var walletsTracked = await GetWallets(x => x.IsActive == true && x.IsLoadBalance == true, x => x.UnixTimeSeconds!);
            await this.LoadClassWallets();
            if (walletsTracked?.Count() > 0) 
            {
                foreach (var walletTracked in walletsTracked)
                {
                    var initialTicks = base.GetInitialTicks(walletTracked?.UnixTimeSeconds);
                    var finalTicks = base.GetFinalTicks();
                    if (initialTicks > finalTicks)
                        initialTicks -= (initialTicks - finalTicks) * 2;
                    var classWallet = this.ClassWallets.FirstOrDefault(x => x.ID == walletTracked!.ClassWalletId);
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
                    walletTracked.UnixTimeSeconds = finalTicks;
                    await base.UpdateUnixTimeSeconds(walletTracked);
                }
            }
            return new ReadWalletsForTransactionCommandResponse { TotalValidTransactions = this.TotalValidTransactions, HasWalletsWithBalanceLoad = hasWalletsWithBalanceLoad };
        }

        private async Task LoadClassWallets() 
        {
            var classWallets = await _classWalletRepository.GetAll();
            foreach (var classWallet in classWallets) 
                this.ClassWallets.Add(classWallet);
        }
    }
}