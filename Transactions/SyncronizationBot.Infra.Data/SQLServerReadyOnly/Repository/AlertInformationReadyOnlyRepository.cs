using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.SQLServerReadyOnly;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Context;
using SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository.Base;



namespace SyncronizationBot.Infra.Data.SQLServerReadyOnly.Repository
{
    public class AlertInformationReadyOnlyRepository : SqlServerReadyOnlyRepository<AlertInformation>, IAlertInformationReadyOnlyRepository
    {
        public AlertInformationReadyOnlyRepository(SqlServerReadyOnlyContext context) : base(context)
        {

        }
    }
}
