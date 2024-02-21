
using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWallet : Entity
    {
        public int? NumberOfBuys { get; set; }
        public int? NumberOfSells { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ValueSpentSol { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ValueSpentUSD { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? QuantityToken { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ValueReceivedSol { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? ValueReceivedUSD { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? QuantityTokenSell { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWalletDescription { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual TokenAlpha? TokenAlpha { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual Wallet? Wallet { get; set; }
    }
}
