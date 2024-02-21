﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Infra.Data.Base.Mapper;


namespace SyncronizationBot.Infra.Data.MongoDB.Mapper
{
    public class TokenAlphaMap : BaseMapper<TokenAlpha>
    {
        public TokenAlphaMap() : base(EDatabase.Mongodb)
        {
        }

        protected override void IgnoreProperties(EntityTypeBuilder<TokenAlpha> builder)
        {
            builder.Ignore(ta => ta.Token);
            builder.Ignore(ta => ta.TokenAlphaConfiguration);
            builder.Ignore(ta => ta.TokenAlphas);
        }

    }
}
