using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Repository.Base.Interfaces;
using SyncronizationBot.Infra.Data.SQLServer.Context;

namespace SyncronizationBot.Infra.Data.SQLServer.Repository.ReadyOnly
{
    public class AlertInformationRepository : SqlServerReadyOnlyRepository<AlertInformation>, IAlertInformationRepository
    {
        public AlertInformationRepository(SqlServerReadyOnlyContext context) : base(context)
        {
        }
    }
}
