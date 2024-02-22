using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenSecurity : Entity
    {
        public Guid? TokenId { get; set; }
        public string? CreatorAddress { get; set; }
        public long? CreationTime { get; set; }
        public DateTime? CreationTimeDate 
        { 
            get 
            {
                if (this.CreationTime != null) 
                    return DateTimeTicks.Instance.ConvertTicksToDateTime(this.CreationTime ?? 0);
                return null!;
            } 
        }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Top10HolderBalance { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Top10HolderPercent { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Top10UserBalance { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? Top10UserPercent { get; set; }
        public bool? IsTrueToken { get; set; }
        public string? LockInfo { get; set; }
        public bool? Freezeable { get; set; }
        public string? FreezeAuthority { get; set; }
        public string? TransferFeeEnable { get; set; }
        public string? TransferFeeData { get; set; }
        public bool? IsToken2022 { get; set; }
        public string? NonTransferable { get; set; }
        public string? MintAuthority { get; set; }
        public bool? IsMutable { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }
    }
}
