﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer;
using SyncronizationBot.Infra.Data.SQLServer.Context;
using SyncronizationBot.Infra.Data.SQLServer.Repository.Base;


namespace SyncronizationBot.Infra.Data.SQLServer.Repository
{
    public class TelegramMessageRepository : SqlServerRepository<TelegramMessage>, ITelegramMessageRepository
    {
        public TelegramMessageRepository(SqlServerContext context) : base(context)
        {

        }
    }
}
