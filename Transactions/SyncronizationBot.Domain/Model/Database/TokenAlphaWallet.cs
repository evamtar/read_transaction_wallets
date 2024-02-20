
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWallet : Entity
    {
        public int? NumberOfBuys { get; set; }
        public int? NumberOfSells { get; set; }
        public decimal? ValueSpentSol { get; set; }
        public decimal? ValueSpentUSD { get; set; }
        public decimal? QuantityToken { get; set; }
        public decimal? ValueReceivedSol { get; set; }
        public decimal? ValueReceivedUSD { get; set; }
        public decimal? QuantityTokenSell { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWalletDescription { get; set; }
        public virtual TokenAlpha? TokenAlpha { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
