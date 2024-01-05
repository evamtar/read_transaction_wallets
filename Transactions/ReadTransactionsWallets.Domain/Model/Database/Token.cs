using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class Token : Entity
    {
        public string? Hash { get; set; }
        public string? TokenAlias { get; set; }
        public string? TypeOfToken { get; set; }
        public int? Decimals { get; set; }

        public virtual List<Transactions>? Transactions { get; set; }
    }
}
