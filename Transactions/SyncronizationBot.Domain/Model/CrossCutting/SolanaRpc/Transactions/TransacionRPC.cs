

using Newtonsoft.Json;

namespace SyncronizationBot.Domain.Model.CrossCutting.SolanaRpc.Transactions
{
    public class TransacionRPC
    {
        [JsonProperty("jsonrpc")]
        public string? Jsonrpc { get; set; }

        [JsonProperty("result")]
        public List<Result?>? Result { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }
    }
    public class Result
    {
        [JsonProperty("blockTime")]
        public long? BlockTime { get; set; }

        [JsonProperty("confirmationStatus")]
        public string? ConfirmationStatus { get; set; }

        [JsonProperty("err")]
        public Err? Err { get; set; }

        [JsonProperty("memo")]
        public object? Memo { get; set; }

        [JsonProperty("signature")]
        public string? Signature { get; set; }

        [JsonProperty("slot")]
        public int? Slot { get; set; }
    }

    public class Err
    {
        [JsonProperty("InstructionError")]
        public List<object?>? InstructionError { get; set; }
    }
}
