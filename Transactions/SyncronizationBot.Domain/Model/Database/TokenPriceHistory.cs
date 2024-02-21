﻿using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenPriceHistory : Entity
    {
        public Guid? TokenId { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? MarketCap { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Liquidity { get; set; }
        public int? UniqueWallet24h { get; set; }
        public int? UniqueWalletHistory24h { get; set; }
        public int? NumberMarkets { get; set; }
        public DateTime? CreateDate { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }
    }
}
