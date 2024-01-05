using ReadTransactionsWallets.Domain.Model.Database.Base;

namespace ReadTransactionsWallets.Domain.Model.Database
{
    public class RunTimeController : Entity
    {
        public int? IdRuntime { get; set; }
        public int? UnixTimeSeconds { get; set; }
        public bool? IsRunning { get; set; }
    }
}
