
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Infra.Data.Base.Context;
using SyncronizationBot.Infra.Data.Base.Repository;

namespace SyncronizationBot.Infra.Data.UnitOfWork
{
    public class UnitOfWorkSqlServer : IUnitOfWorkSqlServer
    {
        #region Context

        private SqlServerContext _context;

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

        public IAlertConfigurationRepository AlertConfigurationRepository => _alertConfigurationRepository ?? (_alertConfigurationRepository = (IAlertConfigurationRepository)new SqlServerRepository<AlertConfiguration>(_context));
        public IAlertInformationRepository AlertInformationRepository => _alertInformationRepository ?? (_alertInformationRepository = (IAlertInformationRepository)new SqlServerRepository<AlertInformation>(_context));
        public IAlertParameterRepository AlertParameterRepository => _alertParameterRepository ?? (_alertParameterRepository = (IAlertParameterRepository)new SqlServerRepository<AlertParameter>(_context));
        public IAlertPriceRepository AlertPriceRepository => _alertPriceRepository ?? (_alertPriceRepository = (IAlertPriceRepository)new SqlServerRepository<AlertPrice>(_context));
        public IClassWalletRepository ClassWalletRepository => _classWalletRepository ?? (_classWalletRepository = (IClassWalletRepository)new SqlServerRepository<ClassWallet>(_context));
        public IRunTimeControllerRepository RunTimeControllerRepository => _runTimeControllerRepository ?? (_runTimeControllerRepository = (IRunTimeControllerRepository)new SqlServerRepository<RunTimeController>(_context));
        public ITelegramChannelRepository TelegramChannelRepository => _telegramChannelRepository ?? (_telegramChannelRepository = (ITelegramChannelRepository)new SqlServerRepository<TelegramChannel>(_context));
        public ITelegramMessageRepository TelegramMessageRepository => _telegramMessageRepository ?? (_telegramMessageRepository = (ITelegramMessageRepository)new SqlServerRepository<TelegramMessage>(_context));
        public ITokenAlphaConfigurationRepository TokenAlphaConfigurationRepository => _tokenAlphaConfigurationRepository ?? (_tokenAlphaConfigurationRepository = (ITokenAlphaConfigurationRepository)new SqlServerRepository<TokenAlphaConfiguration>(_context));
        public ITokenAlphaHistoryRepository TokenAlphaHistoryRepository => _tokenAlphaHistoryRepository ?? (_tokenAlphaHistoryRepository = (ITokenAlphaHistoryRepository)new SqlServerRepository<TokenAlphaHistory>(_context));
        public ITokenAlphaRepository TokenAlphaRepository => _tokenAlphaRepository ?? (_tokenAlphaRepository = (ITokenAlphaRepository)new SqlServerRepository<TokenAlpha>(_context));
        public ITokenAlphaWalletHistoryRepository TokenAlphaWalletHistoryRepository => _tokenAlphaWalletHistoryRepository ?? (_tokenAlphaWalletHistoryRepository = (ITokenAlphaWalletHistoryRepository)new SqlServerRepository<TokenAlphaWallet>(_context));
        public ITokenAlphaWalletRepository TokenAlphaWalletRepository => _tokenAlphaWalletRepository ?? (_tokenAlphaWalletRepository = (ITokenAlphaWalletRepository)new SqlServerRepository<TokenAlphaWallet>(_context));
        public ITokenRepository TokenRepository => _tokenRepository ?? (_tokenRepository = (ITokenRepository)new SqlServerRepository<Token>(_context));
        public ITokenPriceHistoryRepository TokenPriceHistoryRepository => _tokenPriceHistoryRepository ?? (_tokenPriceHistoryRepository = (ITokenPriceHistoryRepository)new SqlServerRepository<TokenPriceHistory>(_context));
        public ITokenSecurityRepository TokenSecurityRepository => _tokenSecurityRepository ?? (_tokenSecurityRepository = (ITokenSecurityRepository)new SqlServerRepository<TokenSecurity>(_context));
        public ITransactionsRepository TransactionsRepository => _transactionsRepository ?? (_transactionsRepository = (ITransactionsRepository)new SqlServerRepository<Transactions>(_context));
        public ITransactionNotMappedRepository TransactionNotMappedRepository => _transactionNotMappedRepository ?? (_transactionNotMappedRepository = (ITransactionNotMappedRepository)new SqlServerRepository<TransactionNotMapped>(_context));
        public ITransactionRPCRecoveryRepository TransactionRPCRecoveryRepository => _transactionRPCRecoveryRepository ?? (_transactionRPCRecoveryRepository = (ITransactionRPCRecoveryRepository)new SqlServerRepository<TransactionRPCRecovery>(_context));
        public ITransactionTokenRepository TransactionTokenRepository => _transactionTokenRepository ?? (_transactionTokenRepository = (ITransactionTokenRepository)new SqlServerRepository<TransactionToken>(_context));
        public ITypeOperationRepository TypeOperationRepository => _typeOperationRepository ?? (_typeOperationRepository = (ITypeOperationRepository)new SqlServerRepository<TypeOperation>(_context));
        public IWalletBalanceHistoryRepository WalletBalanceHistoryRepository => _walletBalanceHistoryRepository ?? (_walletBalanceHistoryRepository = (IWalletBalanceHistoryRepository)new SqlServerRepository<WalletBalanceHistory>(_context));
        public IWalletBalanceRepository WalletBalanceRepository => _walletBalanceRepository ?? (_walletBalanceRepository = (IWalletBalanceRepository)new SqlServerRepository<WalletBalance>(_context));
        public IWalletRepository WalletRepository => _walletRepository ?? (_walletRepository = (IWalletRepository)new SqlServerRepository<Wallet>(_context));

        #endregion

        public UnitOfWorkSqlServer(SqlServerContext context) 
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

        public async Task SaveChangesAsync()
        {
            _context.ChangeTracker.Clear();
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.ChangeTracker.Clear();
            _context.SaveChanges();
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
