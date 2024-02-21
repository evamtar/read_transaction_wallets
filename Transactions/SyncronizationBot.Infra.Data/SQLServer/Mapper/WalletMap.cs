using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class WalletMap : BaseMapper<Wallet>
    {
        public WalletMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasOne(w => w.ClassWallet).WithMany(cw => cw.Wallets).HasForeignKey(w => w.ClassWalletId);
        }

        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallet");
            builder.Property(w => w.ID);
            builder.Property(w => w.Hash);
            builder.Property(w => w.ClassWalletId);
            builder.Property(w => w.IsLoadBalance);
            builder.Property(w => w.DateLoadBalance);
            builder.Property(w => w.IsActive);
            builder.Property(w => w.LastUpdate);
            builder.HasKey(w => w.ID);
        }
    }
}
