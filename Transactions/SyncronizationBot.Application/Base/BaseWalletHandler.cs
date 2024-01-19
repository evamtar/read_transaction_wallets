

using MediatR;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Utils;
using System.Linq.Expressions;

namespace SyncronizationBot.Application.Base
{
    public class BaseWalletHandler
    {
        protected readonly IMediator _mediator;
        protected readonly IWalletRepository _walletRepository;
        public BaseWalletHandler(IMediator mediator,
                                 IWalletRepository walletRepository)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
        }

        protected async Task<IEnumerable<Wallet>> GetWallets(Expression<Func<Wallet, bool>> predicate) 
        {
            return await this._walletRepository.Get(predicate);
        }

        protected async Task<Wallet?> GetWallet(Expression<Func<Wallet, bool>> predicate)
        {
            return await this._walletRepository.FindFirstOrDefault(predicate);
        }

        protected long? GetFinalTicks() => DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.Now);

        protected async Task UpdateUnixTimeSeconds(long? finalTicks, Wallet wallet) 
        {
            wallet.UnixTimeSeconds = finalTicks;
            await this._walletRepository.Edit(wallet);
            try { await this._walletRepository.DetachedItem(wallet); } catch { }
        }
    }
}
