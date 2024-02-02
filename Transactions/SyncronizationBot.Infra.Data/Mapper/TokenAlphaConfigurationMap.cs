using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;

namespace SyncronizationBot.Infra.Data.Mapper
{
    public class TokenAlphaConfigurationMap : IEntityTypeConfiguration<TokenAlphaConfiguration>
    {
        public void Configure(EntityTypeBuilder<TokenAlphaConfiguration> builder)
        {
            builder.ToTable("TokenAlphaConfiguration");
            builder.Property(tac => tac.ID);
            builder.Property(tac => tac.MaxMarketcap).HasConversion<string?>();
            builder.Property(tac => tac.MaxDateOfLaunchDays);
            builder.HasKey(tac => tac.ID);
        }
    }
}
