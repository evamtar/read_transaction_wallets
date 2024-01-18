using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenSecurity : Entity
    {
        public Guid? IdToken { get; set; }
        public string? CreatorAddress { get; set; }
        public long? CreationTime { get; set; }
        public DateTime? CreationTimeDate 
        { 
            get 
            {
                if(this.CreationTimeDate != null)
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.CreationTime ?? 0);
                return null;
            } 
        }
        public decimal? Top10HolderBalance { get; set; }
        public decimal? Top10HolderPercent { get; set; }
        public decimal? Top10UserBalance { get; set; }
        public decimal? Top10UserPercent { get; set; }
        public bool? IsTrueToken { get; set; }
        public string? LockInfo { get; set; }
        public string? Freezeable { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? TransferFeeEnable { get; set; }
        public string? TransferFeeData { get; set; }
        public bool? IsToken2022 { get; set; }
        public string? NonTransferable { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }
        public virtual Token? Token { get; set; }
    }
}
