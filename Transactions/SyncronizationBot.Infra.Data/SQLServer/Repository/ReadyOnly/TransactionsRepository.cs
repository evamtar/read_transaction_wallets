﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class TransactionsRepository : SqlServerReadyOnlyRepository<Transactions>, ITransactionsRepository
    {
        public TransactionsRepository(SqlServerReadyOnlyContext context) : base(context)
        {
        }
    }
}