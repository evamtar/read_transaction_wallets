using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;
using SyncronizationBot.Infra.Data.SQLServer.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Context
{
    public class SqlContext : DbContext
    {
        #region Constructor

        public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

        #endregion

        #region DbSetConfiguration

        public DbSet<AlertConfiguration> AlertsConfigurations { get; set; }
        public DbSet<AlertInformation> AlertsInformations { get; set; }
        public DbSet<AlertParameter> AlertsParameters { get; set; }
        public DbSet<AlertPrice> AlertsPrices { get; set; }
        public DbSet<ClassWallet> ClassWallets { get; set; }
        public DbSet<RunTimeController> RunTimeControllers { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<TelegramMessage> TelegramMessages { get; set; }
        public DbSet<TokenAlphaConfiguration> TokenAlphaConfigurations { get; set; }
        public DbSet<TokenAlphaWalletHistory> TokenAlphaWalletHistories { get; set; }
        public DbSet<TokenAlpha> TokenAlphas { get; set; }
        public DbSet<TokenAlphaHistory> TokenAlphaHistories { get; set; }
        public DbSet<TokenAlphaWallet> TokenAlphaWallets { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPriceHistory> TokenPriceHistories { get; set; }
        public DbSet<TokenSecurity> TokenSecurities { get; set; }
        public DbSet<TransactionNotMapped> TransactionsNotMapped { get; set; }
        public DbSet<TransactionRPCRecovery> TransactionRPCRecoveries { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionToken> TransactionTokens { get; set; }
        public DbSet<TypeOperation> TypeOperations { get; set; }
        public DbSet<WalletBalanceHistory> WalletBalanceHistories { get; set; }
        public DbSet<WalletBalance> WalletBalances { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlertConfigurationMap());
            modelBuilder.ApplyConfiguration(new AlertInformationMap());
            modelBuilder.ApplyConfiguration(new AlertParameterMap());
            modelBuilder.ApplyConfiguration(new AlertPriceMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<ClassWallet>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new BaseMapper<RunTimeController>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new BaseMapper<TelegramChannel>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new TelegramMessageMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaConfiguration>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaHistory>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new TokenAlphaMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaWalletHistory>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new TokenAlphaWalletMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<Token>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new TokenPriceHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenSecurityMap());
            modelBuilder.ApplyConfiguration(new TransactionsMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<TransactionNotMapped>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new TransactionRPCRecoveryMap());
            modelBuilder.ApplyConfiguration(new TransactionTokenMap());
            modelBuilder.ApplyConfiguration(new TypeOperationMap());
            modelBuilder.ApplyConfiguration(new BaseMapper<WalletBalanceHistory>(EDatabase.SqlServer));
            modelBuilder.ApplyConfiguration(new WalletBalanceMap());
            modelBuilder.ApplyConfiguration(new WalletMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }



}
