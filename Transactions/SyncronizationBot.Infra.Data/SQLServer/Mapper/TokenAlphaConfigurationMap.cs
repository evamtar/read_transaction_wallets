﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;

namespace SyncronizationBot.Infra.Data.SQLServer.Mapper
{
    public class TokenAlphaConfigurationMap : BaseMapper<TokenAlphaConfiguration>
    {
        public TokenAlphaConfigurationMap() : base(EDatabase.SqlServer)
        {
        }

        protected override void PropertiesWithConversion(EntityTypeBuilder<TokenAlphaConfiguration> builder)
        {
            builder.Property(tac => tac.MaxMarketcap).HasConversion<string?>();
        }
        
    }
}
