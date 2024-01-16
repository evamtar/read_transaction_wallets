
namespace SyncronizationBot.Domain.Model.CrossCutting.Birdeye.TokenSecurity.Response
{
    public class TokenSecurityResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public ResultData? TokenData { get; set; }
    }

    public class ResultData
    {
        public string? CreatorAddress { get; set; }
        public string? OwnerAddress { get; set; }
        public string? CreationTx { get; set; }
        public long? CreationTime { get; set; }
        public long? CreationSlot { get; set; }
        public string? MintTx { get; set; }
        public long? MintTime { get; set; }
        public long? MintSlot { get; set; }
        public decimal? CreatorBalance { get; set; }
        public decimal? OwnerBalance { get; set; }
        public decimal? OwnerPercentage { get; set; }
        public decimal? CreatorPercentage { get; set; }
        public string? MetaplexUpdateAuthority { get; set; }
        public decimal? MetaplexUpdateAuthorityBalance { get; set; }
        public decimal? MetaplexUpdateAuthorityPercent { get; set; }
        public bool? MutableMetadata { get; set; }
        public decimal? Top10HolderBalance { get; set; }
        public decimal? Top10HolderPercent { get; set; }
        public decimal? Top10UserBalance { get; set; }
        public decimal? Top10UserPercent { get; set; }
        public bool? IsTrueToken { get; set; }
        public decimal? TotalSupply { get; set; }
        public List<object>? PreMarketHolder { get; set; }
        public object? LockInfo { get; set; }
        public object? Freezeable { get; set; }
        public object? FreezeAuthority { get; set; }
        public object? TransferFeeEnable { get; set; }
        public object? TransferFeeData { get; set; }
        public bool IsToken2022 { get; set; }
        public object? NonTransferable { get; set; }
    }
}
