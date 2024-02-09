
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWalletHistory : Entity
    {
        public Guid? TokenAlphaWalletId { get; set; }
        public int? NumberOfBuys { get; set; }
        public decimal? ValueSpentSol { get; set; }
        public decimal? ValueSpentUSDC { get; set; }
        public decimal? ValueSpentUSDT { get; set; }
        public decimal? QuantityToken { get; set; }
        public decimal? RequestValueInSol { get; set; }
        public decimal? RequestValueInUSDC { get; set; }
        public decimal? RequestValueInUSDT { get; set; }
        public decimal? RequestQuantityToken { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWalletDescription { get; set; }
    }
}
