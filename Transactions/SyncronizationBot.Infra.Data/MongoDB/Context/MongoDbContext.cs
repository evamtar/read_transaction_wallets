using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;



namespace SyncronizationBot.Infra.Data.MongoDB.Context
{
    public class MongoDbContext : DbContext
    {
        public readonly string? _connectionString;
        public readonly string? _databaseName;
        #region Constructor

        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) 
        {
            var extension = (MongoOptionsExtension?)options.Extensions.FirstOrDefault(x => x is MongoOptionsExtension);
            this._connectionString = extension?.ConnectionString;
            this._databaseName = extension?.DatabaseName;
        }

        #endregion

        #region DbSetConfiguration

        public DbSet<AlertConfiguration> AlertsConfigurations { get; set; }
        public DbSet<AlertInformation> AlertsInformations { get; set; }
        public DbSet<AlertParameter> AlertParameters { get; set; }
        public DbSet<AlertPrice> AlertPrices { get; set; }
        public DbSet<ClassWallet> ClassWallets { get; set; }
        public DbSet<RunTimeController> RunTimeControllers { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<TelegramMessage> TelegramMessages { get; set; }
        public DbSet<TokenAlphaConfiguration> TokenAlphaConfigurations { get; set; }
        public DbSet<TokenAlphaHistory> TokenAlphaHistories { get; set; }
        public DbSet<TokenAlpha> TokensAlphas { get; set; }
        public DbSet<TokenAlphaWalletHistory> TokenAlphaWalletHistories { get; set; }
        public DbSet<TokenAlphaWallet> TokenAlphaWallets { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPriceHistory> TokenPriceHistories { get; set; }
        public DbSet<TokenSecurity> TokenSecurities { get; set; }
        public DbSet<TransactionNotMapped> TransactionNotMappeds { get; set; }
        public DbSet<TransactionRPCRecovery> TransactionRPCs { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionToken> TransactionsToken { get; set; }
        public DbSet<WalletBalanceHistory> WalletBalanceHistories { get; set; }
        public DbSet<TypeOperation> TypeOperations { get; set; }
        public DbSet<WalletBalance> WalletBalances{ get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        
        
        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BaseMapper<AlertConfiguration>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<AlertInformation>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<AlertParameter>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<AlertPrice>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<ClassWallet>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<RunTimeController>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TelegramChannel>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TelegramMessage>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaConfiguration>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaHistory>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlpha>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaWalletHistory>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenAlphaWallet>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<Token>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenPriceHistory>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TokenSecurity>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TransactionNotMapped>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TransactionRPCRecovery>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<Transactions>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TransactionToken>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<WalletBalanceHistory>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<TypeOperation>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<WalletBalance>(EDatabase.Mongodb));
            modelBuilder.ApplyConfiguration(new BaseMapper<Wallet>(EDatabase.Mongodb));
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
