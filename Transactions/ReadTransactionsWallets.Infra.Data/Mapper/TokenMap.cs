using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadTransactionsWallets.Domain.Model.Database;


namespace ReadTransactionsWallets.Infra.Data.Mapper
{
    public class TokenMap : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable("Token");
            builder.Property(t => t.ID);
            builder.Property(t => t.Hash);
            builder.Property(t => t.TokenAlias);
            builder.Property(t => t.TokenType);
            builder.Property(t => t.FreezeAuthority);
            builder.Property(t => t.MintAuthority);
            builder.Property(t => t.IsMutable);
            builder.Property(t => t.Decimals);
            builder.HasKey(t => t.ID);
        }
    }
}
