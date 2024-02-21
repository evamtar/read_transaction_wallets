using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TransactionRPCRecoveryMap : BaseMapper<TransactionRPCRecovery>
    {
        public TransactionRPCRecoveryMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TransactionRPCRecovery> builder)
        {
            builder.Ignore(tc => tc.Wallet);
        }
        
        
    }
}
