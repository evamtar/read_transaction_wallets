using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers
{
    public class ReadWalletsCommandHandler : IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IClassWalletRepository _classWalletRepository;
        private IMediator _mediator;
        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository,
                                         IClassWalletRepository classWalletRepository)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandResponse> Handle(ReadWalletsCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Wallet> walletsTracked = await this._walletRepository.Get(x => x.IsActive == true && x.IsLoadBalance == true);
            foreach (var walletTracked in walletsTracked)
            {
                walletTracked.ClassWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked.IdClassWallet);
                await this._mediator.Send(new RecoverySaveTransactionsCommand 
                { 
                    WalletId = walletTracked.ID,
                    WalletHash = walletTracked.Hash,
                    IdClassification = walletTracked.ClassWallet?.IdClassification,
                    InitialTicks = request.InitialTicks,
                    FinalTicks = request.FinalTicks
                });
            }
            return new ReadWalletsCommandResponse { };
        }
    }
}