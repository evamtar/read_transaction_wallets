using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Base;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IClassWalletRepository _classWalletRepository;
        
        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository,
                                         IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, config)
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
                var finalTicks = base.GetFinalTicks();
                var classWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked!.IdClassWallet);
                await this._mediator.Send(new RecoverySaveTransactionsCommand
                {
                    WalletId = walletTracked?.ID,
                    WalletHash = walletTracked?.Hash,
                    IdClassification = classWallet?.IdClassification,
                    DateLoadBalance = walletTracked?.DateLoadBalance,
                    InitialTicks = base.GetInitialTicks(walletTracked?.UnixTimeSeconds),
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