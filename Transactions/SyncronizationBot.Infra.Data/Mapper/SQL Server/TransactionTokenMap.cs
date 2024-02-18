using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper.SqlServer
{
    public class TransactionTokenMap : IEntityTypeConfiguration<TransactionToken>
    {
        public void Configure(EntityTypeBuilder<TransactionToken> builder)
        {
            builder.ToTable("TransactionToken");
            builder.Property(tt => tt.ID);
            builder.Property(tt => tt.AmountValue).HasConversion<string?>();
            builder.Property(tt => tt.MtkcapToken).HasConversion<string?>();
            builder.Property(tt => tt.TotalToken).HasConversion<string?>();
            builder.Property(tt => tt.TypeTokenTransaction);
            builder.Property(tt => tt.IsArbitrationOperation);
            builder.Property(tt => tt.IsPoolOperation);
            builder.Property(tt => tt.IsSwapOperation);
            builder.Property(tt => tt.TokenId);
            builder.Property(tt => tt.TransactionId);
            builder.HasOne(tt => tt.Token).WithMany(t => t.TransactionTokens).HasForeignKey(tt => tt.TokenId);
            builder.HasOne(tt => tt.Transactions).WithMany(t => t.TransactionTokens).HasForeignKey(tt => tt.TokenId);
            builder.HasKey(tt => tt.ID);
        }
    }
}
