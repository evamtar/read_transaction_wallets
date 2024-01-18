using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TokenSecurityMap : IEntityTypeConfiguration<TokenSecurity>
    {
        public void Configure(EntityTypeBuilder<TokenSecurity> builder)
        {
            builder.ToTable("TokenSecurity");
            builder.Property(ts => ts.ID);
            builder.Property(ts => ts.IdToken);
            builder.Property(ts => ts.CreatorAddress);
            builder.Property(ts => ts.CreationTime);
            builder.Property(ts => ts.Top10HolderBalance);
            builder.Property(ts => ts.Top10HolderPercent);
            builder.Property(ts => ts.Top10UserBalance);
            builder.Property(ts => ts.Top10UserPercent);
            builder.Property(ts => ts.IsTrueToken);
            builder.Property(ts => ts.LockInfo);
            builder.Property(ts => ts.Freezeable);
            builder.Property(ts => ts.FreezeAuthority);
            builder.Property(ts => ts.TransferFeeEnable);
            builder.Property(ts => ts.TransferFeeData);
            builder.Property(ts => ts.IsToken2022);
            builder.Property(ts => ts.NonTransferable);
            builder.Property(ts => ts.MintAuthority);
            builder.Property(ts => ts.IsMutable);
            builder.Ignore(ts => ts.CreationTimeDate);
            builder.HasOne(ts => ts.Token).WithMany(t => t.TokenSecurities).HasForeignKey(ts => ts.IdToken);
            builder.HasKey(ts => ts.ID);
        }
    }
}
