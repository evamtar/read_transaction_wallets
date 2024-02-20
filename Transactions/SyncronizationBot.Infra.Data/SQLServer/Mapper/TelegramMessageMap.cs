﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;



namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TelegramMessageMap : BaseMapper<TelegramMessage>
    {
        public TelegramMessageMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void RelationsShips(EntityTypeBuilder<TelegramMessage> builder)
        {
            builder.HasOne(tm => tm.TelegramChannel).WithMany(tc => tc.TelegramMessages).HasForeignKey(tm => tm.TelegramChannelId);
        }
        
    }
}
