using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenPriceHistory : Entity
    {
        public Guid? TokenId { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? MarketCap { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Liquidity { get; set; }
        public int? UniqueWallet24h { get; set; }
        public int? UniqueWalletHistory24h { get; set; }
        public int? NumberMarkets { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceUsd { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent5m { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent30m { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent1h { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent4h { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent6h { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceChangePercent24h { get; set; }

        public EFontType FontPrice { get; set; }
        public DateTime? CreateDate { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }
    }
}
