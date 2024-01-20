

namespace SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenCreation.Response
{
    public class TokenCreationResponse
    {
        public ResultData? Data { get; set; }
        public bool Success { get; set; }

        public class ResultData
        {
            public string? TxHash { get; set; }
            public decimal? Slot { get; set; }
            public string? TokenAddress { get; set; }
            public decimal? Decimals { get; set; }
            public string? Owner { get; set; }
        }
    }
}
