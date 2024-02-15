

namespace SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Balance.Response
{
    public class SolnetBalanceResponse
    {
        public bool? IsSuccess { get;set; }
        public DateTime? ExecutionDate { get;set; }
        public List<BalanceResponse>? Result { get; set; }
    }

    public class BalanceResponse 
    { 

    }
}
