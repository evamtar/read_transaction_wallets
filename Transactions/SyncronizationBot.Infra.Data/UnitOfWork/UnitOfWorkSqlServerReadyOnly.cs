using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Infra.Data.Base.Context;
using SyncronizationBot.Infra.Data.Base.Repository;

namespace SyncronizationBot.Infra.Data.UnitOfWork
{
    public class UnitOfWorkSqlServerReadyOnly: IUnitOfWorkSqlServerReadyOnly
    {
        #region Context

        private SqlServerReadyOnlyContext _context;

        #endregion

        #region Variables

        private IAlertConfigurationRepository _alertConfigurationRepository;
        private IAlertInformationRepository _alertInformationRepository;
        private IAlertParameterRepository _alertParameterRepository;
        private IAlertPriceRepository _alertPriceRepository;
        private IClassWalletRepository _classWalletRepository;
        private IRunTimeControllerRepository _runTimeControllerRepository;
        private ITelegramChannelRepository _telegramChannelRepository;
        private ITelegramMessageRepository _telegramMessageRepository;
        private ITokenAlphaConfigurationRepository _tokenAlphaConfigurationRepository;
        private ITokenAlphaHistoryRepository _tokenAlphaHistoryRepository;
        private ITokenAlphaRepository _tokenAlphaRepository;
        private ITokenAlphaWalletHistoryRepository _tokenAlphaWalletHistoryRepository;
        private ITokenAlphaWalletRepository _tokenAlphaWalletRepository;
        private ITokenRepository _tokenRepository;
        private ITokenPriceHistoryRepository _tokenPriceHistoryRepository;
        private ITokenSecurityRepository _tokenSecurityRepository;
        private ITransactionsRepository _transactionsRepository;
        private ITransactionNotMappedRepository _transactionNotMappedRepository;
        private ITransactionRPCRecoveryRepository _transactionRPCRecoveryRepository;
        private ITransactionTokenRepository _transactionTokenRepository;
        private ITypeOperationRepository _typeOperationRepository;
        private IWalletBalanceHistoryRepository _walletBalanceHistoryRepository;
        private IWalletBalanceRepository _walletBalanceRepository;
        private IWalletRepository _walletRepository;

        #endregion

        #region Public Interfaces

        public IAlertConfigurationRepository AlertConfigurationRepository => _alertConfigurationRepository ?? (_alertConfigurationRepository = (IAlertConfigurationRepository)new SqlServerReadyOnlyRepository<AlertConfiguration>(_context));
        public IAlertInformationRepository AlertInformationRepository => _alertInformationRepository ?? (_alertInformationRepository = (IAlertInformationRepository)new SqlServerReadyOnlyRepository<AlertInformation>(_context));
        public IAlertParameterRepository AlertParameterRepository => _alertParameterRepository ?? (_alertParameterRepository = (IAlertParameterRepository)new SqlServerReadyOnlyRepository<AlertParameter>(_context));
        public IAlertPriceRepository AlertPriceRepository => _alertPriceRepository ?? (_alertPriceRepository = (IAlertPriceRepository)new SqlServerReadyOnlyRepository<AlertPrice>(_context));
        public IClassWalletRepository ClassWalletRepository => _classWalletRepository ?? (_classWalletRepository = (IClassWalletRepository)new SqlServerReadyOnlyRepository<ClassWallet>(_context));
        public IRunTimeControllerRepository RunTimeControllerRepository => _runTimeControllerRepository ?? (_runTimeControllerRepository = (IRunTimeControllerRepository)new SqlServerReadyOnlyRepository<RunTimeController>(_context));
        public ITelegramChannelRepository TelegramChannelRepository => _telegramChannelRepository ?? (_telegramChannelRepository = (ITelegramChannelRepository)new SqlServerReadyOnlyRepository<TelegramChannel>(_context));
        public ITelegramMessageRepository TelegramMessageRepository => _telegramMessageRepository ?? (_telegramMessageRepository = (ITelegramMessageRepository)new SqlServerReadyOnlyRepository<TelegramMessage>(_context));
        public ITokenAlphaConfigurationRepository TokenAlphaConfigurationRepository => _tokenAlphaConfigurationRepository ?? (_tokenAlphaConfigurationRepository = (ITokenAlphaConfigurationRepository)new SqlServerReadyOnlyRepository<TokenAlphaConfiguration>(_context));
        public ITokenAlphaHistoryRepository TokenAlphaHistoryRepository => _tokenAlphaHistoryRepository ?? (_tokenAlphaHistoryRepository = (ITokenAlphaHistoryRepository)new SqlServerReadyOnlyRepository<TokenAlphaHistory>(_context));
        public ITokenAlphaRepository TokenAlphaRepository => _tokenAlphaRepository ?? (_tokenAlphaRepository = (ITokenAlphaRepository)new SqlServerReadyOnlyRepository<TokenAlpha>(_context));
        public ITokenAlphaWalletHistoryRepository TokenAlphaWalletHistoryRepository => _tokenAlphaWalletHistoryRepository ?? (_tokenAlphaWalletHistoryRepository = (ITokenAlphaWalletHistoryRepository)new SqlServerReadyOnlyRepository<TokenAlphaWallet>(_context));
        public ITokenAlphaWalletRepository TokenAlphaWalletRepository => _tokenAlphaWalletRepository ?? (_tokenAlphaWalletRepository = (ITokenAlphaWalletRepository)new SqlServerReadyOnlyRepository<TokenAlphaWallet>(_context));
        public ITokenRepository TokenRepository => _tokenRepository ?? (_tokenRepository = (ITokenRepository)new SqlServerReadyOnlyRepository<Token>(_context));
        public ITokenPriceHistoryRepository TokenPriceHistoryRepository => _tokenPriceHistoryRepository ?? (_tokenPriceHistoryRepository = (ITokenPriceHistoryRepository)new SqlServerReadyOnlyRepository<TokenPriceHistory>(_context));
        public ITokenSecurityRepository TokenSecurityRepository => _tokenSecurityRepository ?? (_tokenSecurityRepository = (ITokenSecurityRepository)new SqlServerReadyOnlyRepository<TokenSecurity>(_context));
        public ITransactionsRepository TransactionsRepository => _transactionsRepository ?? (_transactionsRepository = (ITransactionsRepository)new SqlServerReadyOnlyRepository<Transactions>(_context));
        public ITransactionNotMappedRepository TransactionNotMappedRepository => _transactionNotMappedRepository ?? (_transactionNotMappedRepository = (ITransactionNotMappedRepository)new SqlServerReadyOnlyRepository<TransactionNotMapped>(_context));
        public ITransactionRPCRecoveryRepository TransactionRPCRecoveryRepository => _transactionRPCRecoveryRepository ?? (_transactionRPCRecoveryRepository = (ITransactionRPCRecoveryRepository)new SqlServerReadyOnlyRepository<TransactionRPCRecovery>(_context));
        public ITransactionTokenRepository TransactionTokenRepository => _transactionTokenRepository ?? (_transactionTokenRepository = (ITransactionTokenRepository)new SqlServerReadyOnlyRepository<TransactionToken>(_context));
        public ITypeOperationRepository TypeOperationRepository => _typeOperationRepository ?? (_typeOperationRepository = (ITypeOperationRepository)new SqlServerReadyOnlyRepository<TypeOperation>(_context));
        public IWalletBalanceHistoryRepository WalletBalanceHistoryRepository => _walletBalanceHistoryRepository ?? (_walletBalanceHistoryRepository = (IWalletBalanceHistoryRepository)new SqlServerReadyOnlyRepository<WalletBalanceHistory>(_context));
        public IWalletBalanceRepository WalletBalanceRepository => _walletBalanceRepository ?? (_walletBalanceRepository = (IWalletBalanceRepository)new SqlServerReadyOnlyRepository<WalletBalance>(_context));
        public IWalletRepository WalletRepository => _walletRepository ?? (_walletRepository = (IWalletRepository)new SqlServerReadyOnlyRepository<Wallet>(_context));
        #endregion

        public UnitOfWorkSqlServerReadyOnly(SqlServerReadyOnlyContext context)
        {
            this._context = context;
            this._alertConfigurationRepository = null!;
            this._alertInformationRepository = null!;
            this._alertParameterRepository = null!;
            this._alertPriceRepository = null!;
            this._classWalletRepository = null!;
            this._runTimeControllerRepository = null!;
            this._telegramChannelRepository = null!;
            this._telegramMessageRepository = null!;
            this._tokenAlphaConfigurationRepository = null!;
            this._tokenAlphaHistoryRepository = null!;
            this._tokenAlphaRepository = null!;
            this._tokenAlphaWalletHistoryRepository = null!;
            this._tokenAlphaWalletRepository = null!;
            this._tokenRepository = null!;
            this._tokenPriceHistoryRepository = null!;
            this._tokenSecurityRepository = null!;
            this._transactionsRepository = null!;
            this._transactionNotMappedRepository = null!;
            this._transactionRPCRecoveryRepository = null!;
            this._transactionTokenRepository = null!;
            this._typeOperationRepository = null!;
            this._walletBalanceHistoryRepository = null!;
            this._walletBalanceRepository = null!;
            this._walletRepository = null!;
        }

        public void Dispose()
        {
            try
            {
                _context.Dispose();
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
