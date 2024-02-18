using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToCollection(typeof(Wallet).Name);
            builder.Property(w => w.ID);
            builder.Property(w => w.Hash);
            builder.Property(w => w.ClassWalletId);
            builder.Property(w => w.IsLoadBalance);
            builder.Property(w => w.DateLoadBalance);
            builder.Property(w => w.IsActive);
            builder.Property(w => w.LastUpdate);
            builder.HasKey(w => w.ID);
            builder.Ignore(w => w.ClassWallet);
            builder.Ignore(w => w.Transactions);
            builder.Ignore(w => w.TransactionsOldForMapping);
            builder.Ignore(w => w.TransactionsRPCRecovery);
            builder.Ignore(w => w.Balances);
            builder.Ignore(w => w.BalancesSFMCompare);
            builder.Ignore(w => w.TokenAlphas);
        }
    }
}
