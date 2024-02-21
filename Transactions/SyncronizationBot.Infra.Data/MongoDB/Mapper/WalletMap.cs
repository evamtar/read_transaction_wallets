using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class WalletMap : BaseMapper<Wallet>
    {
        public WalletMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<Wallet> builder)
        {
            builder.Ignore(w => w.ClassWallet);
            builder.Ignore(w => w.Transactions);
            builder.Ignore(w => w.TransactionsOldForMapping);
            builder.Ignore(w => w.TransactionsRPCRecovery);
            builder.Ignore(w => w.Balances);
            builder.Ignore(w => w.TokenAlphas);
        }
    }
}
