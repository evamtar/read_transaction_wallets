using MediatR;
using Microsoft.Extensions.Options;
using SyncronizationBot.Domain.Model.Configs;
using SyncronizationBot.Domain.Model.Enum;
using SyncronizationBot.Domain.Repository;
using SyncronizationBot.Service.HostedServices.Base;

namespace SyncronizationBot.Service.HostedServices
{
    public class BalanceWalletsService : BaseService
    {
        public BalanceWalletsService(IMediator mediator,
                                     IRunTimeControllerRepository runTimeControllerRepository,
                                     ITypeOperationRepository typeOperationRepository,
                                     IOptions<SyncronizationBotConfig> syncronizationBotConfig) : base(mediator, runTimeControllerRepository, typeOperationRepository, ETypeService.Balance, syncronizationBotConfig)
        {
        }

        protected override Task DoExecute(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
