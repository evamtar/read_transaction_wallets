﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServer.Base;

namespace SyncronizationBot.Domain.Repository.SQLServer
{
    public interface ITokenPriceHistoryRepository : ISqlServerRepository<TokenPriceHistory>
    {
    }
}
