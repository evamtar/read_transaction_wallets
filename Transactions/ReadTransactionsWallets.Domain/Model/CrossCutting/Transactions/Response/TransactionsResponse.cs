

using ReadTransactionsWallets.Utils;
using System.Text.Json.Nodes;

namespace ReadTransactionsWallets.Domain.Model.CrossCutting.Transactions.Response
{
    public class TransactionsResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public ResultResponse? Result { get; set; }
    }

    public class PaginationResponse 
    { 
        public int? CurrentPage { get; set; }
        public int? TotalPages { get; set; }
    }

    public class ResultResponse 
    { 
        public List<TransactionResponse>? Data { get; set; }
        public PaginationResponse? Pagination { get; set; }
    }

    public class TransactionResponse 
    {
        public decimal? BlockTime { get; set; }
        public string? ConfirmationStatus { get; set; }
        public JsonObject? Err { get; set; }
        public JsonObject? Memo { get; set; }
        public string? Signature { get; set; }
        public decimal? Slot { get; set; }
        public DateTime DateOfTransaction 
        { 
            get 
            {
                if (this.BlockTime.HasValue)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime((long)this.BlockTime.Value);
                else
                    return DateTime.MinValue;
            }  
        }
    }

    
}
