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
            builder.Property(t => t.AmountValue).HasPrecision(38, 20);
            builder.Property(t => t.IdToken);
            builder.Property(t => t.IdWallet);
            builder.Property(t => t.TypeOperation);
            builder.Property(t => t.JsonResponse);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Transactions).HasForeignKey(t => t.IdWallet);
            builder.HasOne(t => t.Token).WithMany(tk => tk.Transactions).HasForeignKey(t => t.IdToken);
            builder.HasKey(t => t.ID);
        }
    }
}
