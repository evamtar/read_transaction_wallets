using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.MongoDB.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Context
{
    public class MongoDbContext : DbContext
    {
        #region Constructor

        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) { }

        #endregion

        #region DbSetConfiguration

        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region Mapper Override

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalletMap());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
