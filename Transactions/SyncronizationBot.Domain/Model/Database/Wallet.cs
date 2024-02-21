using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;


namespace SyncronizationBot.Domain.Model.Database
{
    public class Wallet : Entity
    {
        public string? Hash { get; set; }
        public Guid? ClassWalletId { get; set; }
        public bool? IsLoadBalance { get; set; }
        public DateTime? DateLoadBalance { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdate { get; set; }

        [DbMapper(MongoTarget.Ignore)]
        public virtual ClassWallet? ClassWallet { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual List<Transactions>? Transactions { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual List<TransactionRPCRecovery>? TransactionsRPCRecovery { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual List<WalletBalance>? Balances { get; set; }
        [DbMapper(MongoTarget.Ignore)]
        public virtual List<TokenAlphaWallet>? TokenAlphas { get; set; }
    }
}
