using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Application.Commands.MainCommands.AddUpdate;
using SyncronizationBot.Application.Commands.MainCommands.RecoverySave;
using SyncronizationBot.Application.Handlers.Base;
using SyncronizationBot.Application.Response.MainCommands.AddUpdate;
using SyncronizationBot.Application.Response.MainCommands.RecoverySave;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers.MainCommands.AddUpdate
{
    public class UpdateWalletsBalanceCommandHandler : BaseWalletHandler, IRequestHandler<UpdateWalletsBalanceCommand, UpdateWalletsBalanceCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceRepository _walletBalanceRepository;

        public UpdateWalletsBalanceCommandHandler(IMediator mediator,
                                                  IWalletRepository walletRepository,
                                                  IOptions<SyncronizationBotConfig> config,
                                                  IWalletBalanceRepository walletBalanceRepository) : base(mediator, walletRepository, EFontType.ALL, config)
        {
            this._mediator = mediator;
            this._walletBalanceRepository = walletBalanceRepository;
        }

        public async Task<UpdateWalletsBalanceCommandResponse> Handle(UpdateWalletsBalanceCommand request, CancellationToken cancellationToken)
        {
            var datetimeLimit = DateTime.Now;
            var walletTracked = await GetWallet(x => x.IsActive == true && x.IsLoadBalance == true && (x.LastUpdate == null || x.LastUpdate <= datetimeLimit) && x.IsRunningProcess == false, x => x.DateLoadBalance!);
            var hasNext = walletTracked != null;
            while (hasNext)
            {
                var balance = await this._walletBalanceRepository.FindFirstOrDefault(x => x.WalletId == walletTracked!.ID && x.TokenId != null && x.IsActive == true && x.LastUpdate!.Value.AddHours(1) < datetimeLimit);
                var hasNextBalance = balance != null;
                while (hasNextBalance) 
                {
                    var token = await _mediator.Send(new RecoverySaveTokenCommand { TokenHash = balance!.TokenHash! });
                    balance.TotalValueUSD = balance.Quantity * (token.Price ?? 0);
                    balance.LastUpdate = DateTime.Now;
                    await this._walletBalanceRepository.Edit(balance);
                    await this._walletBalanceRepository.DetachedItem(balance);
                    balance = await this._walletBalanceRepository.FindFirstOrDefault(x => x.WalletId == walletTracked!.ID && x.TokenId != null && x.IsActive == true && x.LastUpdate!.Value.AddHours(1) < datetimeLimit);
                    hasNextBalance = balance != null;
                }
                walletTracked = await GetWallet(x => x.IsActive == true && x.IsLoadBalance == true && (x.LastUpdate == null || x.LastUpdate <= datetimeLimit));
                hasNext = walletTracked != null;
            }
            return new UpdateWalletsBalanceCommandResponse { };
        }
    }
}
