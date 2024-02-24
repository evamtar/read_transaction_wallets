using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;



namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class AlertParameterReadyOnlyRepository : SqlServerReadyOnlyRepository<AlertParameter>, IAlertParameterReadyOnlyRepository
    {
        public AlertParameterReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
