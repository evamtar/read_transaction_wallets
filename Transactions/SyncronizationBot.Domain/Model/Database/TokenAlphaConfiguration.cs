using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaConfiguration : Entity
    {
        public decimal? MaxMarketcap { get; set; }
        public int? MaxDateOfLaunchDays { get; set; }
    }
}
