using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadTransactionsWallets.Domain.Model.Database;


namespace ReadTransactionsWallets.Infra.Data.Mapper
{
    public class ClassWalletMap : IEntityTypeConfiguration<ClassWallet>
    {
        public void Configure(EntityTypeBuilder<ClassWallet> builder)
        {
            builder.ToTable("ClassWallet");
            builder.Property(cw => cw.ID);
            builder.Property(cw => cw.IdClassification);
            builder.Property(cw => cw.Description);
            builder.HasKey(cw => cw.ID);
        }
    }
}
