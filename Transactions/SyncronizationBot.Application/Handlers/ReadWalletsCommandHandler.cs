using MediatR;
using SyncronizationBot.Application.Base;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IClassWalletRepository _classWalletRepository;
        
        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository): base(mediator, walletRepository)
        {
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandResponse> Handle(ReadWalletsCommand request, CancellationToken cancellationToken)
        {
            var walletsTracked = await base.GetWallets(x => x.IsActive == true && x.IsLoadBalance == true);
            foreach (var walletTracked in walletsTracked)
            {
                var finalTicks = base.GetFinalTicks();
                walletTracked.ClassWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked.IdClassWallet);
                await this._mediator.Send(new RecoverySaveTransactionsCommand 
                { 
                    WalletId = walletTracked.ID,
                    WalletHash = walletTracked.Hash,
                    IdClassification = walletTracked.ClassWallet?.IdClassification,
                    InitialTicks = walletTracked.UnixTimeSeconds ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now.AddMinutes(-10)),
                    FinalTicks = finalTicks ?? DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now)
                });
                await base.UpdateUnixTimeSeconds(finalTicks, walletTracked);
            }
            return new ReadWalletsCommandResponse { };
        }
    }
}