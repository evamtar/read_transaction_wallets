using SyncronizationBot.Domain.Service.HostedWork;
using SyncronizationBot.Service.HostedServices.Base;


namespace SyncronizationBot.Service.HostedServices
{
    public class BalanceWalletsUpdateService : BaseHostedService<IBalanceWalletsUpdateWork>
    {
        public BalanceWalletsUpdateService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }
    }
}
