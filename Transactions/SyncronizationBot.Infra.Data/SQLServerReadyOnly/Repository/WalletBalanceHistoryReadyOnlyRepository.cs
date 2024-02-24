﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;

namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class WalletBalanceHistoryReadyOnlyRepository : SqlServerReadyOnlyRepository<WalletBalanceHistory>, IWalletBalanceHistoryReadyOnlyRepository
    {
        public WalletBalanceHistoryReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}