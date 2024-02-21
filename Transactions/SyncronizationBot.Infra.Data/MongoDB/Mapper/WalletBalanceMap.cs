using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class WalletBalanceMap : BaseMapper<WalletBalance>
    {
        public WalletBalanceMap() : base(EDatabase.Mongodb)
        {
        }

        
        protected override void IgnoreProperties(EntityTypeBuilder<WalletBalance> builder)
        {
            builder.Ignore(wb => wb.Wallet);
            builder.Ignore(wb => wb.Token);
        }

    }
}
