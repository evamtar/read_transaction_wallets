using MediatR;
using SyncronizationBot.Application.Commands;
using SyncronizationBot.Application.Response;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;

namespace SyncronizationBot.Application.Handlers
{
    public class RecoveryAddUpdateBalanceItemCommandHandler : IRequestHandler<RecoveryAddUpdateBalanceItemCommand, RecoveryAddUpdateBalanceItemCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IWalletBalanceRepository _walletBalanceRepository;
        public RecoveryAddUpdateBalanceItemCommandHandler(IMediator mediator,
                                                          IWalletBalanceRepository walletBalanceRepository)
        {
            this._mediator = mediator;
            this._walletBalanceRepository = walletBalanceRepository;
        }

        public async Task<RecoveryAddUpdateBalanceItemCommandResponse> Handle(RecoveryAddUpdateBalanceItemCommand request, CancellationToken cancellationToken)
        {
            var balance = await this._walletBalanceRepository.FindFirstOrDefault( x => x.IdToken == request.TokenId && x.IdWallet == request.WalleId );
            var percentage = (decimal?)100;
            if (balance == null)
            {
                var price = await this._mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
                balance = await this._walletBalanceRepository.Add(new WalletBalance
                {
                    IdWallet = request.WalleId,
                    IdToken = request.TokenId,
                    TokenHash = request.TokenHash,
                    Quantity = request.Quantity,
                    Price = price?.Data?[request.TokenHash!].Price ?? 0,
                    TotalValueUSD = request.Quantity * price?.Data?[request.TokenHash!].Price,
                    IsActive = request.Quantity > 0,
                    LastUpdate = DateTime.Now
                });
            }
            else 
            {
                percentage = this.CalculatePercentege(request.Quantity, balance.Quantity);
                balance.Quantity += request.Quantity;
                balance.IsActive = balance.Quantity > 0;
                if (balance.IsActive ?? false) 
                {
                    var price = await this._mediator.Send(new RecoveryPriceCommand { Ids = new List<string> { request.TokenHash! } });
                    balance.Price = price?.Data?[request.TokenHash!].Price ?? 0;
                    balance.TotalValueUSD = request.Quantity * price?.Data?[request.TokenHash!].Price;
                }
                balance.LastUpdate = DateTime.Now;
                balance = await this._walletBalanceRepository.Edit(balance);
                try { await this._walletBalanceRepository.DetachedItem(balance); } catch { }
            }
            return new RecoveryAddUpdateBalanceItemCommandResponse 
            {
                Quantity = balance.Quantity,
                Price = balance.Price,
                PercentModify = percentage,
                IsActive = balance.IsActive
            };
        }

        private decimal? CalculatePercentege(decimal? quantityEnter, decimal? quantity) 
        {
            if (quantity - quantityEnter <= 0)
                return -100;
            else 
                return quantityEnter / quantity * 100;
        }
    }
}
