using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class WalletBalanceHistoryMap : BaseMapper<WalletBalanceHistory>
    {
        public WalletBalanceHistoryMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<WalletBalanceHistory> builder)
        {
            builder.Property(wbh => wbh.OldQuantity).HasConversion<string?>();
            builder.Property(wbh => wbh.NewQuantity).HasConversion<string?>();
            builder.Property(wbh => wbh.RequestQuantity).HasConversion<string?>();
            builder.Property(wbh => wbh.PercentageCalculated).HasConversion<string?>();
            builder.Property(wbh => wbh.Price).HasConversion<string?>();
            builder.Property(wbh => wbh.TotalValueUSD).HasConversion<string?>();
        }

        
    }
}
