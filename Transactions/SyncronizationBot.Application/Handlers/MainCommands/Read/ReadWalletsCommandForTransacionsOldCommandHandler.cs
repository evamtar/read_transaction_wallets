using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Commands.MainCommands.Read;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Commands.SolanaFM;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.Read;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.Read
{
    public class ReadWalletsCommandForTransacionsOldCommandHandler : BaseWalletHandler, IRequestHandler<ReadWalletsCommandForTransacionsOldCommand, ReadWalletsCommandForTransacionsOldCommandResponse>
    {
        private readonly ITransactionsOldForMappingRepository _transactionsOldForMappingRepository;
        private readonly IClassWalletRepository _classWalletRepository;

        public ReadWalletsCommandForTransacionsOldCommandHandler(IMediator mediator,
                                                                 IWalletRepository walletRepository,
                                                                 IClassWalletRepository classWalletRepository,
                                                                 ITransactionsOldForMappingRepository transactionsOldForMappingRepository,
                                                                 IOptions<SyncronizationBotConfig> config) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._transactionsOldForMappingRepository = transactionsOldForMappingRepository;
            this._classWalletRepository = classWalletRepository;
        }
        public async Task<ReadWalletsCommandForTransacionsOldCommandResponse> Handle(ReadWalletsCommandForTransacionsOldCommand request, CancellationToken cancellationToken)
        {
            var walletsTracked = await GetWallets(x => x.IsActive == true, x => x.IsLoadBalance!);
            if (walletsTracked != null) 
            {
                foreach (var walletTracked in walletsTracked)
                {
                    var classWallet = await this._classWalletRepository.FindFirstOrDefault(x => x.ID == walletTracked.ClassWalletId);
                    var saveTransactionsOldForMappingResponse = await this._mediator.Send(new RecoverySaveTransactionsOldForMappingCommand { WalletId = walletTracked.ID!, WalletHash = walletTracked.Hash!, ClassWallet = classWallet, DateLoadBalance = walletTracked?.DateLoadBalance ?? DateTime.Now });
                    var transactionsOldForMapping = await this._mediator.Send(new RecoveryTransactionsSignatureForAddressCommand { WalletId = walletTracked.ID!, WalletHash = walletTracked.Hash!, DateLoadBalance = walletTracked?.DateLoadBalance ?? DateTime.Now, Limit = (walletTracked?.IsLoadBalance ?? false)? 50: 1000 });
                }
            }
            
            return new ReadWalletsCommandForTransacionsOldCommandResponse {};
        }
    }
}