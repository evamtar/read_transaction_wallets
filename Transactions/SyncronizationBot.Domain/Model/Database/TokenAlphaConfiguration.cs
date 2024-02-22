﻿using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaConfiguration : Entity
    {
        public string? Name {get;set;}
        public int? Ordernation { get;set;}

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, convertionType:typeof(string))]
        public decimal? MaxMarketcap { get; set; }
        public int? MaxDateOfLaunchDays { get; set; }
        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<TokenAlpha>? TokenAlphas { get; set; }
    }
}
