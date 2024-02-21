using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlpha : Entity
    {
        public int? CallNumber { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? InitialMarketcap { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ActualMarketcap { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? InitialPrice { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ActualPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }
        public string? TokenSymbol { get; set; }
        public string? TokenName { get; set; }
        public Guid? TokenAlphaConfigurationId { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual TokenAlphaConfiguration? TokenAlphaConfiguration { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual List<TokenAlphaWallet>? TokenAlphas { get; set; }
    }
}
