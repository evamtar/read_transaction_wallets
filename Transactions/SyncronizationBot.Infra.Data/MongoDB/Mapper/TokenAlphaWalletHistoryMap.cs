﻿using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenAlphaWalletHistoryMap : BaseMapper<TokenAlphaWalletHistory>
    {
        public TokenAlphaWalletHistoryMap() : base(EDatabase.Mongodb)
        {
        }

        
    }
}
