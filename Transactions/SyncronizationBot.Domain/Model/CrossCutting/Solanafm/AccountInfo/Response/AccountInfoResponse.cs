namespace SyncronizationBot.Domain.Model.CrossCutting.Solanafm.AccountInfo.Response
{
    public class AccountInfoResponse
    {
        public Guid? Id { get; set; }
        public string? Jsonrpc { get; set; }
        public ResultResponse? Result { get; set; }
    }

    public class ResultResponse
    {
        public ContextResponse? Context { get; set; }
        public ValueResponse? Value { get; set; }
    }

    public class ContextResponse
    {
        public string? ApiVersion { get; set; }
        public long? Slot { get; set; }
    }

    public class ValueResponse
    {
        public List<string>? Data { get; set; }
        public bool? Executable { get; set; }
        public long? Lamports { get; set; }
        public string? Owner { get; set; }
        public decimal? RentEpoch { get; set; }
        public int? Space { get; set; }
    }
}
