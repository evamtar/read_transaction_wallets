using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TransactionTokenMap : BaseMapper<TransactionToken>
    {
        public TransactionTokenMap() : base(EDatabase.SqlServer)
        {
        }
        
        protected override void RelationsShips(EntityTypeBuilder<TransactionToken> builder)
        {
            builder.HasOne(tt => tt.Token).WithMany(t => t.TransactionTokens).HasForeignKey(tt => tt.TokenId);
            builder.HasOne(tt => tt.Transactions).WithMany(t => t.TransactionTokens).HasForeignKey(tt => tt.TokenId);
        }
    }
}
