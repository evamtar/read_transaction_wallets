using MediatR;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using SyncronizationBot.Application.Commands;
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
        private ConcurrentBag<Wallet> WalletsUpdated = new ConcurrentBag<Wallet>();
        private ConcurrentBag<Wallet> WalletsRetry = new ConcurrentBag<Wallet>();
        private bool? HasWalletsWithBalanceLoad = false;
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
                var existsWalletsToProcess = true;
                while (existsWalletsToProcess) 
                {
                    await ProcessWallets(request, walletsTracked);
                    if(this.WalletsRetry.Count > 0) 
                    { 
                        walletsTracked!.Clear();
                        walletsTracked = this.GetWalletsToRetry();
                        await Task.Delay(200);
                    }
                    else
                        existsWalletsToProcess = false;
                }
                //Atualizar as wallets
                foreach (var wallet in this.WalletsUpdated) 
                    await base.UpdateUnixTimeSeconds(wallet);
            }
            return new ReadWalletsForTransactionCommandResponse { TotalValidTransactions = this.TotalValidTransactions, HasWalletsWithBalanceLoad = hasWalletsWithBalanceLoad };
        }

        private async Task ProcessWallets(ReadWalletsForTransactionCommand request, List<Wallet>? walletsTracked) 
        {
            if (walletsTracked?.Count() > 0)
            {
                await Parallel.ForEachAsync(walletsTracked, async (walletTracked, cancellationToken) =>
                {
                    try
                    {
                        HasWalletsWithBalanceLoad = true;
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
                        WalletsUpdated.Add(walletTracked!);
                    }
                    catch
                    {
                        WalletsRetry.Add(walletTracked);
                    }
                });
            }
        }

        private List<Wallet>? GetWalletsToRetry() 
        { 
            var listWallets = new List<Wallet>();
            foreach (var wallet in this.WalletsRetry)
                listWallets.Add(wallet);
            this.WalletsRetry.Clear();
            return listWallets;
        }

        private async Task LoadClassWallets() 
        {
            var classWallets = await _classWalletRepository.GetAll();
            foreach (var classWallet in classWallets) 
                this.ClassWallets.Add(classWallet);
        }
    }
}