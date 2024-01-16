using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.Mapper;


namespace SyncronizationBot.Infra.Data.Context
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
        public DbSet<Token> Tokens { get; set; }
        public DbSet<WalletBalance> WalletBalances { get; set; }
        public DbSet<TelegramChannel> TelegramChannels { get; set; }
        public DbSet<TransactionNotMapped> TransactionsNotMapped { get; set; }
        public DbSet<AlertPrice> AlertPrices { get; set; }

        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassWalletMap());
            modelBuilder.ApplyConfiguration(new RunTimeControllerMap());
            modelBuilder.ApplyConfiguration(new TokenMap());
            modelBuilder.ApplyConfiguration(new TransactionsMap());
            modelBuilder.ApplyConfiguration(new WalletMap());
            modelBuilder.ApplyConfiguration(new WalletBalanceMap());
            modelBuilder.ApplyConfiguration(new TelegramChannelMap());
            modelBuilder.ApplyConfiguration(new TransactionNotMappedMap());
            modelBuilder.ApplyConfiguration(new AlertPriceMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }



}
