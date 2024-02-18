using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;

namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadWalletsCommandForTransacionOldCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommandForTransacionOldCommand, ReadWalletsCommandForTransacionOldCommandResponse>
    {
        private readonly ITransactionOldForMappingRepository _transactionOldForMappingRepository;
        private readonly IClassWalletRepository _classWalletRepository;

        public ReadWalletsCommandForTransacionOldCommandHandler(IMediator mediator,
                                                                 IWalletRepository walletRepository,
                                                                 IClassWalletRepository classWalletRepository,
                                                                 ITransactionOldForMappingRepository transactionOldForMappingRepository,
                                                                 IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._transactionOldForMappingRepository = transactionOldForMappingRepository;
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandForTransacionOldCommandResponse> Handle(ReadWalletsCommandForTransacionOldCommand request, CancellationToken cancellationToken)
        {
            var walletsTracked = await GetWallets(x => x.IsActive == true, x => x.IsLoadBalance!);
            if (walletsTracked != null) 
            {
                foreach (var walletTracked in walletsTracked)
                {
                    var classWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked.ClassWalletId);
                    var saveTransactionOldForMappingResponse = await this._mediator.Send(new RecoverySaveTransactionOldForMappingCommand { WalletId = walletTracked.ID!, WalletHash = walletTracked.Hash!, ClassWallet = classWallet, DateLoadBalance = walletTracked?.DateLoadBalance ?? DateTime.Now });
                    var transactionOldForMapping = await this._mediator.Send(new RecoveryTransactionsSignatureForAddressCommand { WalletId = walletTracked.ID!, WalletHash = walletTracked.Hash!, DateLoadBalance = walletTracked?.DateLoadBalance ?? DateTime.Now, Limit = (walletTracked?.IsLoadBalance ?? false)? 50: 1000 });
                }
            }
            
            return new ReadWalletsCommandForTransacionOldCommandResponse {};
        }
    }
}