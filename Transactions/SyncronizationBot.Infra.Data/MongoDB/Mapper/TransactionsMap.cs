using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TransactionsMap : BaseMapper<Transactions>
    {
        public TransactionsMap() : base(EDatabase.Mongodb)
        {
        }

       
        protected override void IgnoreProperties(EntityTypeBuilder<Transactions> builder)
        {
            builder.Ignore(t => t.WalletHash);
            builder.Ignore(t => t.ClassWallet);
            builder.Ignore(t => t.TypeOperation);
            builder.Ignore(t => t.Wallet);
            builder.Ignore(t => t.TransactionTokens);
        }
    }
}
