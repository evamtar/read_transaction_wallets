

using SyncronizationBot.Domain.Repository.Base.Interfaces;

namespace SyncronizationBot.Domain.Repository.UnitOfWork
{
    public interface IUnitOfWorkSqlServer : IDisposable
    {
        IWalletRepository WalletRepository { get; }
        IWalletBalanceRepository WalletBalanceRepository { get; }
        IWalletBalanceHistoryRepository WalletBalanceHistoryRepository { get; }
        ITypeOperationRepository TypeOperationRepository { get; }
        ITransactionTokenRepository TransactionTokenRepository { get; }
        ITransactionRPCRecoveryRepository TransactionRPCRecoveryRepository { get; }
        ITransactionNotMappedRepository TransactionNotMappedRepository { get; }
        ITransactionsRepository TransactionsRepository { get; }
        ITokenSecurityRepository TokenSecurityRepository { get; }
        ITokenPriceHistoryRepository TokenPriceHistoryRepository { get; }
        ITokenRepository TokenRepository { get; }
        ITokenAlphaWalletRepository TokenAlphaWalletRepository { get; }
        ITokenAlphaWalletHistoryRepository TokenAlphaWalletHistoryRepository { get; }
        ITokenAlphaRepository TokenAlphaRepository { get; }
        ITokenAlphaHistoryRepository TokenAlphaHistoryRepository { get; }
        ITokenAlphaConfigurationRepository TokenAlphaConfigurationRepository { get; }
        ITelegramMessageRepository TelegramMessageRepository { get; }
        ITelegramChannelRepository TelegramChannelRepository { get; }
        IRunTimeControllerRepository RunTimeControllerRepository { get; }
        IClassWalletRepository ClassWalletRepository { get; }
        IAlertPriceRepository AlertPriceRepository { get; }
        IAlertParameterRepository AlertParameterRepository { get; }
        IAlertInformationRepository AlertInformationRepository { get; }
        IAlertConfigurationRepository AlertConfigurationRepository { get; }

        Task SaveChangesAsync();
        void SaveChanges();
    }
}
