using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaConfiguration : Entity
    {
        public string? Name {get;set;}
        public int? Ordernation { get;set;}
        public decimal? MaxMarketcap { get; set; }
        public int? MaxDateOfLaunchDays { get; set; }
        public virtual List<TokenAlpha>? TokenAlphas { get; set; }
    }
}
