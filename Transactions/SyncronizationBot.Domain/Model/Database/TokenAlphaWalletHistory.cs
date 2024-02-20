
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWalletHistory : Entity
    {
        public Guid? TokenAlphaWalletId { get; set; }
        public int? NumberOfBuys { get; set; }
        public int? NumberOfSells { get; set; }
        public decimal? ValueSpentSol { get; set; }
        public decimal? ValueSpentUSD { get; set; }
        public decimal? QuantityToken { get; set; }
        public decimal? ValueReceivedSol { get; set; }
        public decimal? ValueReceivedUSD { get; set; }
        public decimal? QuantityTokenSell { get; set; }
        public decimal? RequestValueInSol { get; set; }
        public decimal? RequestValueInUSD { get; set; }
        public decimal? RequestQuantityToken { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWalletDescription { get; set; }
    }
}
