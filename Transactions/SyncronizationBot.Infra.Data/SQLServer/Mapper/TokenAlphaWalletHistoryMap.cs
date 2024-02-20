using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenAlphaWalletHistoryMap : BaseMapper<TokenAlphaWalletHistory>
    {
        public TokenAlphaWalletHistoryMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlphaWalletHistory> builder)
        {
            builder.Property(tawh => tawh.ValueSpentSol).HasConversion<string?>();
            builder.Property(tawh => tawh.ValueSpentUSD).HasConversion<string?>();
            builder.Property(tawh => tawh.QuantityToken).HasConversion<string?>();
            builder.Property(tawh => tawh.ValueReceivedSol).HasConversion<string?>();
            builder.Property(tawh => tawh.ValueReceivedUSD).HasConversion<string?>();
            builder.Property(tawh => tawh.QuantityTokenSell).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestValueInSol).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestValueInUSD).HasConversion<string?>();
            builder.Property(tawh => tawh.RequestQuantityToken).HasConversion<string?>();
        }
    }
}
