using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TransactionsMap : BaseMapper<Transactions>
    {
        public TransactionsMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<Transactions> builder)
        {
            builder.Property(t => t.FeeTransaction).HasConversion<string?>();
            builder.Property(t => t.PriceSol).HasConversion<string?>();
            builder.Property(t => t.TotalOperationSol).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.WalletId);
            builder.HasOne(t => t.TypeOperation).WithMany(w => w.Transactions).HasForeignKey(t => t.TypeOperationId);
        }

        protected override void IgnoreProperties(EntityTypeBuilder<Transactions> builder)
        {
            builder.Ignore(t => t.WalletHash);
            builder.Ignore(t => t.ClassWallet);
        }
    }
}
