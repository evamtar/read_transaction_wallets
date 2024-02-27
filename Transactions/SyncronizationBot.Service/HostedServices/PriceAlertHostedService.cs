using MediatR;
using SyncronizationBot.Domain.Service.HostedWork;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class PriceAlertHostedService : BaseHostedService<IPriceAlertWork>
    {

        public PriceAlertHostedService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

    }
}
