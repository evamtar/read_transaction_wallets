using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlpha : Entity
    {
        public int? CallNumber { get; set; }
        public decimal? InitialMarketcap { get; set; }
        public decimal? ActualMarketcap { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public Guid? TokenId { get; set; }
        public string? TokenHash { get; set; }
        public string? TokenSymbol { get; set; }
        public string? TokenName { get; set; }
        public Guid? TokenAlphaConfigurationId { get; set; }
        public virtual Token? Token { get; set; }
        public virtual TokenAlphaConfiguration? TokenAlphaConfiguration { get; set; }
        public virtual List<TokenAlphaWallet>? TokenAlphas { get; set; }
    }
}
