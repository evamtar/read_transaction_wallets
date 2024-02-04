
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWallet : Entity
    {
        public int? NumberOfBuys { get; set; }
        public decimal? ValueSpentSol { get; set; }
        public decimal? ValueSpentUSDC { get; set; }
        public decimal? ValueSpentUSDT { get; set; }
        public decimal? QuantityToken { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public virtual TokenAlpha? TokenAlpha { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
