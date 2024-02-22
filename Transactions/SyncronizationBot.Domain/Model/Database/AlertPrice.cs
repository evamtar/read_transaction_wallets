﻿using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;


namespace SyncronizationBot.Domain.Model.Database
{
    public class AlertPrice : Entity
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceBase { get; set; }
        public string? TokenHash { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasConvertion, typeof(string))]
        public decimal? PriceValue { get; set; }

        [DbSqlServerMapper(SqlServerTarget.HasPrecision, precision:5, scale:2)]
        public decimal? PricePercent { get; set; }
        public ETypeAlertPrice TypeAlert { get; set; }
        public bool? IsRecurrence { get; set; }
        public Guid? TelegramChannelId { get; set; }

        [DbMongoMapper(MongoTarget.Ignore)]
        public virtual TelegramChannel? TelegramChannel { get; set; }
    }
}
