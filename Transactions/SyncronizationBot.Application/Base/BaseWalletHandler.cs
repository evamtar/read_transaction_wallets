

using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
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
        protected readonly IOptions<SyncronizationBotConfig> _config;
        public BaseWalletHandler(IMediator mediator,
                                 IWalletRepository walletRepository,
                                 IOptions<SyncronizationBotConfig> config)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
            this._config = config;
        }

        protected async Task<IEnumerable<Wallet>> GetWallets(Expression<Func<Wallet, bool>> predicate) 
        {
            return await this._walletRepository.Get(predicate);
        }

        protected async Task<Wallet?> GetWallet(Expression<Func<Wallet, bool>> predicate, Expression<Func<Wallet, object>> keySelector = null!)
        {
            return await this._walletRepository.FindFirstOrDefault(predicate, keySelector);
        }

        protected long GetInitialTicks(decimal? initialTicks) 
        {
            var dateAjusted = DateTimeTicks.Instance.ConvertTicksToDateTime((long?)initialTicks ?? 0).AddMinutes(this._config.Value.UTCTransactionMinutesAdjust ?? -5);
            return DateTimeTicks.Instance.ConvertDateTimeToTicks(dateAjusted);

        }

        protected long GetFinalTicks() => DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.UtcNow);

        protected async Task UpdateUnixTimeSeconds(long? finalTicks, Wallet wallet) 
        {
            wallet.UnixTimeSeconds = finalTicks;
            await this._walletRepository.Edit(wallet);
            try { await this._walletRepository.DetachedItem(wallet); } catch { }
        }
    }
}
