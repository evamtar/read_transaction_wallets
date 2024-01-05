

namespace ReadTransactionsWallets.Infra.CrossCutting.Transactions.Configs
{
    public class TransactionsConfig
    {
        public string? BaseUrl { get; set; }
        public string? ParametersUrl { get; set; }
        public bool? Inflow { get; set; }
        public bool? Outflow { get; set; }
        public string? ApiKey { get; set; }
    }
}
