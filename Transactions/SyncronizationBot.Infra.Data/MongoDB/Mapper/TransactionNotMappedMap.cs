﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    [Obsolete]
    public class TransactionNotMappedMap : BaseMapper<TransactionNotMapped>
    {
        public TransactionNotMappedMap() : base(EDatabase.Mongodb)
        {
        }
    }
}
