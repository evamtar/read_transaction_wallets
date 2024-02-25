using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class AlertPriceRepository : SqlServerReadyOnlyRepository<AlertPrice>, IAlertPriceRepository
    {
        public AlertPriceRepository(SqlServerReadyOnlyContext context) : base(context)
        {
        }
    }
}
