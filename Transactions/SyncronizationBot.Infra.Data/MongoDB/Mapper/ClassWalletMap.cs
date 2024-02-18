using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class ClassWalletMap : IEntityTypeConfiguration<ClassWallet>
    {
        public void Configure(EntityTypeBuilder<ClassWallet> builder)
        {
            builder.ToCollection(typeof(ClassWallet).Name);
            builder.Property(cw => cw.ID);
            builder.Property(cw => cw.IdClassification);
            builder.Property(cw => cw.Description);
            builder.HasKey(cw => cw.ID);
        }
    }
}
