using Newtonsoft.Json;
using SyncronizationBot.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncronizationBot.Domain.Model.CrossCutting.SolnetRpc.Token
{
    public class JsonRpcResponse
    {
        [JsonProperty("jsonrpc")]
        public string? JsonRpc { get; set; }

        [JsonProperty("result")]
        public Result? Result { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }
    }

    public class Result
    {
        [JsonProperty("context")]
        public Context? Context { get; set; }

        [JsonProperty("value")]
        public Value? Value { get; set; }
    }

    public class Context
    {
        [JsonProperty("apiVersion")]
        public string? ApiVersion { get; set; }

        [JsonProperty("slot")]
        public ulong Slot { get; set; }
    }

    public class Value
    {
        [JsonProperty("data")]
        public Data? Data { get; set; }

        [JsonProperty("executable")]
        public bool Executable { get; set; }

        [JsonProperty("lamports")]
        public long Lamports { get; set; }

        [JsonProperty("owner")]
        public string? Owner { get; set; }

        [JsonProperty("rentEpoch")]
        public decimal? RentEpoch { get; set; }

        [JsonProperty("space")]
        public int Space { get; set; }
    }

    public class Data
    {
        [JsonProperty("parsed")]
        public Parsed? Parsed { get; set; }

        [JsonProperty("program")]
        public string? Program { get; set; }

        [JsonProperty("space")]
        public int Space { get; set; }
    }

    public class Parsed
    {
        [JsonProperty("info")]
        public Info? Info { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }

    public class Info
    {
        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        [JsonProperty("freezeAuthority")]
        public string? FreezeAuthority { get; set; }

        [JsonProperty("isInitialized")]
        public bool IsInitialized { get; set; }

        [JsonProperty("mintAuthority")]
        public string? MintAuthority { get; set; }

        [JsonProperty("supply")]
        public string? Supply { get; set; }

        public decimal? SupplyDecimal => this.Supply?.ToDecimal(this.Decimals);
    }
}
