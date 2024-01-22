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
            builder.Property(t => t.MtkcapTokenSource).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenSourcePool).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenDestination).HasConversion<string?>();
            builder.Property(t => t.MtkcapTokenDestinationPool).HasConversion<string?>();
            builder.Property(t => t.PriceSol).HasConversion<string?>();
            builder.Property(t => t.IdTokenSource);
            builder.Property(t => t.IdTokenSourcePool);
            builder.Property(t => t.IdTokenDestination);
            builder.Property(t => t.IdTokenDestinationPool);
            builder.Property(t => t.IdWallet);
            builder.Property(t => t.TypeOperation);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.IdWallet);
            builder.HasOne(t => t.TokenSource).WithMany(tk => tk.TransactionsSource).HasForeignKey(t => t.IdTokenSource);
            builder.HasOne(t => t.TokenSourcePool).WithMany(tk => tk.TransactionsSourcePool).HasForeignKey(t => t.IdTokenSourcePool);
            builder.HasOne(t => t.TokenDestination).WithMany(tk => tk.TransactionsDestination).HasForeignKey(t => t.IdTokenDestination);
            builder.HasOne(t => t.TokenDestinationPool).WithMany(tk => tk.TransactionsDestinationPool).HasForeignKey(t => t.IdTokenDestinationPool);
            builder.HasKey(t => t.ID);
        }
    }
}
