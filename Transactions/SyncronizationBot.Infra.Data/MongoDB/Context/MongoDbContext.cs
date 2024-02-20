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

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TypeOperation> TypeOperations { get; set; }
        public DbSet<ClassWallet> ClassWallets { get; set; }
        public DbSet<RunTimeController> RunTimeControllers  { get; set; }

        #endregion

        #region Mapper Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalletMap());
            modelBuilder.ApplyConfiguration(new RunTimeControllerMap());
            modelBuilder.ApplyConfiguration(new ClassWalletMap());
            modelBuilder.ApplyConfiguration(new TypeOperationMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
