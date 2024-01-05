using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadTransactionsWallets.Domain.Model.Database;


namespace ReadTransactionsWallets.Infra.Data.Mapper
{
    public class TransactionsMap : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.ToTable("Transactions");
            builder.Property(t => t.ID);
            builder.Property(t => t.Signature);
            builder.Property(t => t.DateOfTransaction);
            builder.Property(t => t.AmountValueSource).HasPrecision(38, 20);
            builder.Property(t => t.AmountValueDestination).HasPrecision(38, 20);
            builder.Property(t => t.IdTokenSource);
            builder.Property(t => t.IdTokenDestination);
            builder.Property(t => t.IdWallet);
            builder.Property(t => t.TypeOperation);
            builder.Property(t => t.JsonResponse);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.IdWallet);
            builder.HasOne(t => t.TokenSource).WithMany(tk => tk.TransactionsSource).HasForeignKey(t => t.IdTokenSource);
            builder.HasOne(t => t.TokenDestination).WithMany(tk => tk.TransactionsDestination).HasForeignKey(t => t.IdTokenDestination);
            builder.HasKey(t => t.ID);
        }
    }
}
