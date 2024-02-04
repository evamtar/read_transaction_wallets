using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TokenAlphaHistory : Entity
    {
        public Guid? TokenAlphaId { get; set; }
        public int? CallNumber { get; set; }
        public decimal? InitialMarketcap { get; set; }
        public decimal? ActualMarketcap { get; set; }
        public decimal? InitialPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool? IsCalledInChannel { get; set; }
        public Guid? TokenId { get; set; }
        public Guid? TokenAlphaConfigurationId { get; set; }
    }
}
