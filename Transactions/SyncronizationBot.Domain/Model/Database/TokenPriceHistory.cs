using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenPriceHistory : Entity
    {
        public Guid? TokenId { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? Liquidity { get; set; }
        public int? UniqueWallet24h { get; set; }
        public int? UniqueWalletHistory24h { get; set; }
        public int? NumberMarkets { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual Token? Token { get; set; }
    }
}
