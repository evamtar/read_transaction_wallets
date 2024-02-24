﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly.Base;


namespace SyncronizationBot.Domain.Repository.SQLServerReadyOnly
{
    public interface IClassWalletReadyOnlyRepository : ISqlServerReadyOnlyRepository<ClassWallet>
    {
    }
}
