﻿using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.SQLServer.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Context
{
    public class SqlContext : DbContext
    {
        #region Constructor

        public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

        #endregion

        #region DbSetConfiguration

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<ClassWallet> ClassWallets { get; set; }
        public DbSet<RunTimeController> RunTimeControllers { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionNotMapped> TransactionsNotMapped { get; set; }
        public DbSet<TransactionOldForMapping> TransactionsOldForMappings { get; set; }
        public DbSet<TransactionRPCRecovery> TransactionsContingencies { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenSecurity> TokenSecurities { get; set; }
        public DbSet<TokenAlpha> TokenAlphas { get; set; }
        public DbSet<TokenAlphaHistory> TokenAlphaHistories { get; set; }
        public DbSet<TokenAlphaWallet> TokenAlphaWallets { get; set; }
        public DbSet<TokenAlphaWalletHistory> TokenAlphaWalletHistories { get; set; }
        public DbSet<TokenAlphaConfiguration> TokenAlphaConfigurations { get; set; }
        public DbSet<WalletBalance> WalletBalances { get; set; }
        public DbSet<WalletBalanceSFMCompare> WalletBalancesSFMCompare { get; set; }
        public DbSet<WalletBalanceHistory> WalletBalanceHistories { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<AlertPrice> AlertsPrices { get; set; }
        public DbSet<AlertConfiguration> AlertsConfigurations { get; set; }
        public DbSet<AlertInformation> AlertsInformations { get; set; }
        public DbSet<AlertParameter> AlertsParameters { get; set; }
        public DbSet<TelegramMessage> TelegramMessages { get; set; }
        public DbSet<PublishMessage> PublishMessages { get; set; }
        public DbSet<TransactionToken> TransactionTokens { get; set; }
        public DbSet<TypeOperation> TypeOperations { get; set; }

        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassWalletMap());
            modelBuilder.ApplyConfiguration(new RunTimeControllerMap());
            modelBuilder.ApplyConfiguration(new TokenMap());
            modelBuilder.ApplyConfiguration(new TokenSecurityMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaWalletMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaWalletHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaConfigurationMap());
            modelBuilder.ApplyConfiguration(new TypeOperationMap());
            modelBuilder.ApplyConfiguration(new TransactionsMap());
            modelBuilder.ApplyConfiguration(new TransactionTokenMap());
            modelBuilder.ApplyConfiguration(new TransactionsOldForMappingMap());
            modelBuilder.ApplyConfiguration(new TransactionRPCRecoveryMap());
            modelBuilder.ApplyConfiguration(new TransactionNotMappedMap());
            modelBuilder.ApplyConfiguration(new WalletMap());
            modelBuilder.ApplyConfiguration(new WalletBalanceMap());
            modelBuilder.ApplyConfiguration(new WalletBalanceSFMCompareMap());
            modelBuilder.ApplyConfiguration(new WalletBalanceHistoryMap());
            modelBuilder.ApplyConfiguration(new TelegramChannelMap());
            modelBuilder.ApplyConfiguration(new TelegramMessageMap());
            modelBuilder.ApplyConfiguration(new AlertPriceMap());
            modelBuilder.ApplyConfiguration(new AlertConfigurationMap());
            modelBuilder.ApplyConfiguration(new AlertInformationMap());
            modelBuilder.ApplyConfiguration(new AlertParameterMap());
            modelBuilder.ApplyConfiguration(new PublishMessageMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }



}