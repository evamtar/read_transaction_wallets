

namespace ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Request
{
    public class TransactionsRequest
    {
        public string? WalletPublicKey { get; set; }
        public decimal? UtcFrom { get; set; }
        public decimal? UtcTo { get; set; }
        public int? Page { get; set; }
    }
}
