﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class AlertPriceMap : BaseMapper<AlertPrice>
    {
        public AlertPriceMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<AlertPrice> builder)
        {
            builder.Ignore(ap => ap.TelegramChannel);
        }
    }
}
