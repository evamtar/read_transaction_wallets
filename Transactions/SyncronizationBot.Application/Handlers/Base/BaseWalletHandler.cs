

using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
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

        protected async Task<IEnumerable<Wallet>> GetWallets(Expression<Func<Wallet, bool>> predicate)
        {
            return await _walletRepository.Get(predicate);
        }

        protected async Task<Wallet?> GetWallet(Expression<Func<Wallet, bool>> predicate, Expression<Func<Wallet, object>> keySelector = null!)
        {
            var wallet = await _walletRepository.FindFirstOrDefault(predicate, keySelector);
            if (wallet == null) return wallet;
            wallet.IsRunningProcess = true;
            wallet = await this._walletRepository.Edit(wallet!);
            return wallet;
        }

        protected long GetInitialTicks(decimal? initialTicks)
        {
            var dateAjusted = DateTimeTicks.Instance.ConvertTicksToDateTime((long?)initialTicks ?? 0).AddMinutes(_config.Value.UTCTransactionMinutesAdjust ?? -5);
            return DateTimeTicks.Instance.ConvertDateTimeToTicks(dateAjusted);

        }

        protected long GetFinalTicks() => DateTimeTicks.Instance.ConvertDateTimeToTicks(DateTime.UtcNow);

        protected async Task UpdateUnixTimeSeconds(long? finalTicks, Wallet wallet)
        {
            wallet.UnixTimeSeconds = finalTicks;
            wallet.IsRunningProcess = false;
            await _walletRepository.Edit(wallet);
            await _walletRepository.DetachedItem(wallet);
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
                    return this._config.Value.SaveBalance == ESaveBalance.Byrdeye;
                case EFontType.SOLANA_FM:
                    return this._config.Value.SaveBalance == ESaveBalance.SolanaFM;
                case EFontType.SOLANA_BEACH:
                case EFontType.DEXSCREENER:
                case EFontType.JUPITER:
                default:
                    return false;
            }
        }

    }
}
