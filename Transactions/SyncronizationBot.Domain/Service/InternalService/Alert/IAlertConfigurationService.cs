using SyncronizationBot.Domain.Model.Database;
using SyncronizationBot.Domain.Service.InternalService.Base;

namespace SyncronizationBot.Domain.Service.InternalService.Alert
{
    public interface IAlertConfigurationService : ICachedServiceBase<AlertConfiguration>
    {
    }
}
