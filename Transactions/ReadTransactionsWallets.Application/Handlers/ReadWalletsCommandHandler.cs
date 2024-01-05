using MediatR;
using ReadTransactionsWallets.Application.Commands;
using ReadTransactionsWallets.Application.Response;
using ReadTransactionsWallets.Domain.Model.Database;
using ReadTransactionsWallets.Domain.Repository;

namespace ReadTransactionsWallets.Application.Handlers
{
    public class ReadWalletsCommandHandler : IRequestHandler<ReadWalletsCommand, ReadWalletsCommandResponse>
    {
        private readonly IWalletRepository _walletRepository;
        private IMediator _mediator;
        public ReadWalletsCommandHandler(IMediator mediator,
                                         IWalletRepository walletRepository)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
        }
        public async Task<ReadWalletsCommandResponse> Handle(ReadWalletsCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Wallet> walletsTracked = await this._walletRepository.GetAll();
            foreach (var walletTracked in walletsTracked)
            {
                await this._mediator.Send(new RecoverySaveTransactionsCommand 
                { 
                    WalletId = walletTracked.ID,
                    WalletHash = walletTracked.Hash,
                    InitialTicks = request.InitialTicks,
                    FinalTicks = request.FinalTicks
                });
            }
            return new ReadWalletsCommandResponse { };
        }
    }
}