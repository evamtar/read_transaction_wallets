using SyncronizationBot.Domain.Service.HostedWork;
using SyncronizationBot.Service.HostedServices.Base;


namespace SyncronizationBot.Service.HostedServices
{
    public class BalanceWalletsHostedService : BaseHostedService<IBalanceWalletsWork>
    {
        public BalanceWalletsHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
