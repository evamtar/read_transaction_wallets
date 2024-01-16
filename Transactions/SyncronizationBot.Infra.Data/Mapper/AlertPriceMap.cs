﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;


namespace SyncronizationBot.Infra.Data.Mapper
{
    public class AlertPriceMap : IEntityTypeConfiguration<AlertPrice>
    {
        public void Configure(EntityTypeBuilder<AlertPrice> builder)
        {
            builder.ToTable("AlertPrice");
            builder.Property(ap => ap.ID);
            builder.Property(ap => ap.CreateDate);
            builder.Property(ap => ap.EndDate);
            builder.Property(ap => ap.PriceBase);
            builder.Property(ap => ap.TokenHash);
            builder.Property(ap => ap.PriceValue);
            builder.Property(ap => ap.PricePercent);
            builder.Property(ap => ap.TelegramChannelId);
            builder.HasOne(ap => ap.TelegramChannel).WithMany(tc => tc.AlertPrices).HasForeignKey(ap => ap.TelegramChannelId);
            builder.HasKey(ap => ap.ID);
        }
    }
}
