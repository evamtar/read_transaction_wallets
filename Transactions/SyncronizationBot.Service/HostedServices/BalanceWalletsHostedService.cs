using MediatR;
using SyncronizationBot.Domain.Service.InternalService.HostedWork;
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
