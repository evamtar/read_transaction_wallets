using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TransactionsMap : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.ToTable("Transactions");
            builder.Property(t => t.ID);
            builder.Property(t => t.Signature);
            builder.Property(t => t.DateOfTransaction);
            builder.Property(t => t.AmountValueSource).HasConversion<string?>();
            builder.Property(t => t.AmountValueSourcePool).HasConversion<string?>();
            builder.Property(t => t.AmountValueDestination).HasConversion<string?>();
            builder.Property(t => t.AmountValueDestinationPool).HasConversion<string?>();
            builder.Property(t => t.FeeTransaction).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenSource).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenSourcePool).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenDestination).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenDestinationPool).HasConversion<string?>();
            builder.Property(t => t.PriceTokenSourceUSD).HasConversion<string?>();
            builder.Property(t => t.PriceTokenSourcePoolUSD).HasConversion<string?>();
            builder.Property(t => t.PriceTokenDestinationUSD).HasConversion<string?>();
            builder.Property(t => t.PriceTokenDestinationPoolUSD).HasConversion<string?>();
            builder.Property(t => t.PriceSol).HasConversion<string?>();
            builder.Property(t => t.TotalTokenSource).HasConversion<string?>();
            builder.Property(t => t.TotalTokenSourcePool).HasConversion<string?>();
            builder.Property(t => t.TotalTokenDestination).HasConversion<string?>();
            builder.Property(t => t.TotalTokenDestinationPool).HasConversion<string?>();
            builder.Property(t => t.TokenSourceId);
            builder.Property(t => t.TokenSourcePoolId);
            builder.Property(t => t.TokenDestinationId);
            builder.Property(t => t.TokenDestinationPoolId);
            builder.Property(t => t.WalletId);
            builder.Property(t => t.TypeOperation);
            builder.Ignore(t => t.WalletHash);
            builder.Ignore(t => t.ClassWallet);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.WalletId);
            builder.HasOne(t => t.TokenSource).WithMany(tk => tk.TransactionsSource).HasForeignKey(t => t.TokenSourceId);
            builder.HasOne(t => t.TokenSourcePool).WithMany(tk => tk.TransactionsSourcePool).HasForeignKey(t => t.TokenSourcePoolId);
            builder.HasOne(t => t.TokenDestination).WithMany(tk => tk.TransactionsDestination).HasForeignKey(t => t.TokenDestinationId);
            builder.HasOne(t => t.TokenDestinationPool).WithMany(tk => tk.TransactionsDestinationPool).HasForeignKey(t => t.TokenDestinationPoolId);
            builder.HasKey(t => t.ID);
        }
    }
}
