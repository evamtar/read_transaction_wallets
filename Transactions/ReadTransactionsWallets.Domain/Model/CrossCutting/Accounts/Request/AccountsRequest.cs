

using Newtonsoft.Json;

namespace ReadTransactionsWallets.Domain.Model.CrossCutting.Accounts.Request
{
    public class AccountsRequest
    {
        [JsonProperty("accountHashes")]
        public List<string>? AccountHashes { get; set; }
    }
}
