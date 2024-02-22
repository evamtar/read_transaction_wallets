using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;

namespace SyncronizationBot.Domain.Model.Database
{
    public class TelegramChannel : Entity
    {
        [DbSqlServerMapper(SqlServerTarget.HasPrecision, precision:30, scale:2)]
        public decimal? ChannelId { get; set; }
        public string? ChannelName { get; set; }
        public int? TimeBeforeDelete { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<AlertPrice>? AlertPrices { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<AlertConfiguration>? AlertsConfigurations { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual List<TelegramMessage>? TelegramMessages { get; set; }
    }
}
