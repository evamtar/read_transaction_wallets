using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper.SqlServer
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
