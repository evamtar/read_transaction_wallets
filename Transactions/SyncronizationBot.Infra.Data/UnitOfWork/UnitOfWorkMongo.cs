using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Domain.Repository.UnitOfWork;
using SyncronizationBot.Infra.Data.MongoDB.Context;
using SyncronizationBot.Infra.Data.MongoDB.Repository;


namespace SyncronizationBot.Infra.Data.UnitOfWork
{
    public class UnitOfWorkMongo : IUnitOfWorkMongo
    {
        #region Context

        private MongoDbContext _context;

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

        public IAlertConfigurationRepository AlertConfigurationRepository => _alertConfigurationRepository ?? (_alertConfigurationRepository = new AlertConfigurationRepository(_context));
        public IAlertInformationRepository AlertInformationRepository => _alertInformationRepository ?? (_alertInformationRepository = new AlertInformationRepository(_context));
        public IAlertParameterRepository AlertParameterRepository => _alertParameterRepository ?? (_alertParameterRepository = new AlertParameterRepository(_context));
        public IAlertPriceRepository AlertPriceRepository => _alertPriceRepository ?? (_alertPriceRepository = new AlertPriceRepository(_context));
        public IClassWalletRepository ClassWalletRepository => _classWalletRepository ?? (_classWalletRepository = new ClassWalletRepository(_context));
        public IRunTimeControllerRepository RunTimeControllerRepository => _runTimeControllerRepository ?? (_runTimeControllerRepository = new RunTimeControllerRepository(_context));
        public ITelegramChannelRepository TelegramChannelRepository => _telegramChannelRepository ?? (_telegramChannelRepository = new TelegramChannelRepository(_context));
        public ITelegramMessageRepository TelegramMessageRepository => _telegramMessageRepository ?? (_telegramMessageRepository = new TelegramMessageRepository(_context));
        public ITokenAlphaConfigurationRepository TokenAlphaConfigurationRepository => _tokenAlphaConfigurationRepository ?? (_tokenAlphaConfigurationRepository = new TokenAlphaConfigurationRepository(_context));
        public ITokenAlphaHistoryRepository TokenAlphaHistoryRepository => _tokenAlphaHistoryRepository ?? (_tokenAlphaHistoryRepository = new TokenAlphaHistoryRepository(_context));
        public ITokenAlphaRepository TokenAlphaRepository => _tokenAlphaRepository ?? (_tokenAlphaRepository = new TokenAlphaRepository(_context));
        public ITokenAlphaWalletHistoryRepository TokenAlphaWalletHistoryRepository => _tokenAlphaWalletHistoryRepository ?? (_tokenAlphaWalletHistoryRepository = new TokenAlphaWalletHistoryRepository(_context));
        public ITokenAlphaWalletRepository TokenAlphaWalletRepository => _tokenAlphaWalletRepository ?? (_tokenAlphaWalletRepository = new TokenAlphaWalletRepository(_context));
        public ITokenRepository TokenRepository => _tokenRepository ?? (_tokenRepository = new TokenRepository(_context));
        public ITokenPriceHistoryRepository TokenPriceHistoryRepository => _tokenPriceHistoryRepository ?? (_tokenPriceHistoryRepository = new TokenPriceHistoryRepository(_context));
        public ITokenSecurityRepository TokenSecurityRepository => _tokenSecurityRepository ?? (_tokenSecurityRepository = new TokenSecurityRepository(_context));
        public ITransactionsRepository TransactionsRepository => _transactionsRepository ?? (_transactionsRepository = new TransactionsRepository(_context));
        public ITransactionNotMappedRepository TransactionNotMappedRepository => _transactionNotMappedRepository ?? (_transactionNotMappedRepository = new TransactionNotMappedRepository(_context));
        public ITransactionRPCRecoveryRepository TransactionRPCRecoveryRepository => _transactionRPCRecoveryRepository ?? (_transactionRPCRecoveryRepository = new TransactionRPCRecoveryRepository(_context));
        public ITransactionTokenRepository TransactionTokenRepository => _transactionTokenRepository ?? (_transactionTokenRepository = new TransactionTokenRepository(_context));
        public ITypeOperationRepository TypeOperationRepository => _typeOperationRepository ?? (_typeOperationRepository = new TypeOperationRepository(_context));
        public IWalletBalanceHistoryRepository WalletBalanceHistoryRepository => _walletBalanceHistoryRepository ?? (_walletBalanceHistoryRepository = new WalletBalanceHistoryRepository(_context));
        public IWalletBalanceRepository WalletBalanceRepository => _walletBalanceRepository ?? (_walletBalanceRepository = new WalletBalanceRepository(_context));
        public IWalletRepository WalletRepository => _walletRepository ?? (_walletRepository = new WalletRepository(_context));

        #endregion

        public UnitOfWorkMongo(MongoDbContext context)
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
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
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
