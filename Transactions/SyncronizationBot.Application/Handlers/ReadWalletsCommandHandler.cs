using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IClassWalletRepository _classWalletRepository;
        
        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository,
                                         IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandResponse> Handle(ReadWalletsCommand request, CancellationToken cancellationToken)
        {
            var datetimeLimit = DateTime.Now;
            var walletTracked = await base.GetWallet(x => x.IsActive == true && x.IsLoadBalance == true && (x.LastUpdate == null || x.LastUpdate <= datetimeLimit), x=> x.UnixTimeSeconds!);
            var hasNext = walletTracked != null;
            while (hasNext) 
            {
                var initialTicks = base.GetInitialTicks(walletTracked?.UnixTimeSeconds);
                var finalTicks = base.GetFinalTicks();
                if (initialTicks > finalTicks)
                    initialTicks -= (initialTicks - finalTicks) * 2;
                var classWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked!.IdClassWallet);
                await this._mediator.Send(new RecoverySaveTransactionsCommand
                {
                    WalletId = walletTracked?.ID,
                    WalletHash = walletTracked?.Hash,
                    IdClassification = classWallet?.IdClassification,
                    DateLoadBalance = walletTracked?.DateLoadBalance,
                    IsContingecyTransactions = base.IsContingencyTransactions,
                    InitialTicks = initialTicks,
                    FinalTicks = finalTicks
                });
                walletTracked!.LastUpdate = DateTime.Now;
                await base.UpdateUnixTimeSeconds(finalTicks, walletTracked);
                walletTracked = await base.GetWallet(x => x.IsActive == true && x.IsLoadBalance == true && (x.LastUpdate == null || x.LastUpdate <= datetimeLimit));
                hasNext = walletTracked != null;
            }
            return new ReadWalletsCommandResponse { };
        }
    }
}