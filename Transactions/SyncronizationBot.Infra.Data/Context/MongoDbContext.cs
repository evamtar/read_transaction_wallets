using Microsoft.EntityFrameworkCore;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Infra.Data.Mapper.MongoDB;

namespace SyncronizationBot.Infra.Data.Context
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalletMap());
        }

        #endregion
    }
}
