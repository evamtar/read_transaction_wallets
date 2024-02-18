

using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Utils;
using System.Linq.Expressions;

namespace SyncronizationBot.Application.Handlers.Base
{
    public class BaseWalletHandler
    {
        protected readonly IMediator _mediator;
        protected readonly IWalletRepository _walletRepository;
        protected readonly EFontType _fontType;
        protected readonly IOptions<SyncronizationBotConfig> _config;
        public BaseWalletHandler(IMediator mediator,
                                 IWalletRepository walletRepository,
                                 EFontType fontType,
                                 IOptions<SyncronizationBotConfig> config)
        {
            this._mediator = mediator;
            this._walletRepository = walletRepository;
            this._fontType = fontType;
            this._config = config;
        }

        protected int TotalValidTransactions { get; set; } = 0;

        protected async Task<List<Wallet>> GetWallets(Expression<Func<Wallet, bool>> predicate)
        {
            return await _walletRepository.GetAsync(predicate);
        }

        protected async Task<List<Wallet>?> GetWallets(Expression<Func<Wallet, bool>> predicate, Expression<Func<Wallet, object>> keySelector = null!)
        {
            return await _walletRepository.GetAsync(predicate, keySelector);
        }


        protected long GetInitialTicks(decimal? initialTicks)
        {
            var dateAjusted = DateTimeTicks.Instance.ConvertTicksToDateTime((long?)initialTicks ?? 0).AddMinutes(_config.Value.UTCTransactionMinutesAdjust ?? -5);
            return DateTimeTicks.Instance.ConvertDateTimeToTicks(dateAjusted);
        }

        protected long GetFinalTicks() 
        {
            return DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.UtcNow);
        }

        protected async Task UpdateUnixTimeSeconds(long? finalTicks, Wallet wallet)
        {
            await _walletRepository.UpdateAsync(wallet);
            await _walletRepository.DetachedItemAsync(wallet);
        }
        protected async Task UpdateUnixTimeSeconds(Wallet wallet) 
        {
            await this.UpdateUnixTimeSeconds((long?)null, wallet);
        }

        protected decimal? GetDivisor(int? decimals)
        {
            string number = "1";
            for (int i = 0; i < decimals; i++)
                number += "0";
            return decimal.Parse(number);
        }

        protected bool? IsSaveBalance() 
        {
            switch (this._fontType)
            {
                case EFontType.BIRDEYE:
                case EFontType.SOLANA_FM:
                case EFontType.SOLANA_BEACH:
                case EFontType.DEXSCREENER:
                case EFontType.JUPITER:
                default:
                    return false;
            }
        }

    }
}
