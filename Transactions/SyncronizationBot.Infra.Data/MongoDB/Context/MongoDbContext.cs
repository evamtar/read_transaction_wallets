using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.MongoDB.Mapper;



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
        public DbSet<AlertPrice> AlertPrices { get; set; }
        public DbSet<ClassWallet> ClassWallets { get; set; }
        public DbSet<RunTimeController> RunTimeControllers { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<TelegramMessage> TelegramMessages { get; set; }
        public DbSet<TokenAlphaConfiguration> TokenAlphaConfigurations { get; set; }
        public DbSet<TokenAlphaHistory> TokenAlphaHistories { get; set; }
        public DbSet<TokenAlpha> TokenAlphas { get; set; }
        public DbSet<TokenAlphaWalletHistory> TokenAlphaWalletHistories { get; set; }
        public DbSet<TokenAlphaWallet> TokenAlphaWallets { get; set; }
        public DbSet<TokenPriceHistory> TokenPriceHistories { get; set; }
        public DbSet<TokenSecurity> TokenSecurities { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TypeOperation> TypeOperations { get; set; }
        
        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlertConfigurationMap());
            modelBuilder.ApplyConfiguration(new AlertInformationMap());
            modelBuilder.ApplyConfiguration(new AlertPriceMap());
            modelBuilder.ApplyConfiguration(new ClassWalletMap());
            modelBuilder.ApplyConfiguration(new RunTimeControllerMap());
            modelBuilder.ApplyConfiguration(new TelegramChannelMap());
            modelBuilder.ApplyConfiguration(new TelegramMessageMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaConfigurationMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaWalletHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenAlphaWalletMap());
            modelBuilder.ApplyConfiguration(new TokenMap());
            modelBuilder.ApplyConfiguration(new TokenPriceHistoryMap());
            modelBuilder.ApplyConfiguration(new TokenSecurityMap());


            modelBuilder.ApplyConfiguration(new TypeOperationMap());
            modelBuilder.ApplyConfiguration(new WalletMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
