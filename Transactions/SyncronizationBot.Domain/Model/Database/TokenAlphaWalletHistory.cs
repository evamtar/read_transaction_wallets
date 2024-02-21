
using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaWalletHistory : Entity
    {
        public Guid? TokenAlphaWalletId { get; set; }
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

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? RequestValueInSol { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? RequestValueInUSD { get; set; }

        [DbMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? RequestQuantityToken { get; set; }
        public Guid? TokenAlphaId { get; set; }
        public Guid? WalletId { get; set; }
        public string? WalletHash { get; set; }
        public string? ClassWalletDescription { get; set; }
    }
}
