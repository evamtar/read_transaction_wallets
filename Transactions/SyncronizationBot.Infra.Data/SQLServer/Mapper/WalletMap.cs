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
    }
}
