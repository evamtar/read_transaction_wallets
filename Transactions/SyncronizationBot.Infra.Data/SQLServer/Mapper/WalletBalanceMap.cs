﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class WalletBalanceMap : BaseMapper<WalletBalance>
    {
        public WalletBalanceMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<WalletBalance> builder)
        {
            builder.Property(wb => wb.Quantity).HasConversion<string?>();
            builder.Property(wb => wb.Price).HasConversion<string?>();
            builder.Property(wb => wb.TotalValueUSD).HasConversion<string?>();
        }

        protected override void RelationsShips(EntityTypeBuilder<WalletBalance> builder)
        {
            builder.HasOne(wb => wb.Wallet).WithMany(w => w.Balances).HasForeignKey(wb => wb.WalletId);
            builder.HasOne(wb => wb.Token).WithMany(t => t.Balances).HasForeignKey(wb => wb.TokenId);
        }

    }
}
