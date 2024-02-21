using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TransactionRPCRecoveryMap : BaseMapper<TransactionRPCRecovery>
    {
        public TransactionRPCRecoveryMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TransactionRPCRecovery> builder)
        {
            builder.HasOne(tc => tc.Wallet).WithMany(w => w.TransactionsRPCRecovery).HasForeignKey(tc => tc.WalletId);
        }
        
    }
}
