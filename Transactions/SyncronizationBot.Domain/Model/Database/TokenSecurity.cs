using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Utils;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenSecurity : Entity
    {
        public Guid? TokenId { get; set; }
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

        public string? FreezeAuthority { get; set; }
        public string? MintAuthority { get; set; }
        
        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual Token? Token { get; set; }
    }
}
